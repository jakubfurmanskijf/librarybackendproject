@echo off
setlocal

REM --- CONFIG ---
set BASE=https://localhost:7221
set CURL=curl.exe
set HERE=%~dp0

REM --- Create tiny JSON files next to the script ---
> "%HERE%book.json"      echo {"isbn":"9780132350884","title":"Clean Code","author":"Robert C. Martin","year":2008,"totalCopies":2,"availableCopies":2}
> "%HERE%member.json"    echo {"fullName":"Bob Reader","email":"bob@example.com"}
> "%HERE%borrow.json"    echo {"memberId":1,"bookId":1,"days":14}
> "%HERE%return.json"    echo {"borrowingId":1}

echo == AUTH (admin) ==
for /f "tokens=* usebackq" %%t in (`
  %CURL% -k -s -X POST "%BASE%/api/Auth/login" ^
    -H "Content-Type: application/json" ^
    -d "{ ""username"":""admin"", ""password"":""admin123"" }"
`) do set TOKEN=%%t
echo Got token (first 40): %TOKEN:~0,40%...

set AUTH=Authorization: Bearer %TOKEN%

echo.
echo == Books: GET all ==
%CURL% -k -s "%BASE%/api/Books" -H "%AUTH%"
echo.

echo == Books: POST (create) ==
%CURL% -k -s -X POST "%BASE%/api/Books" -H "%AUTH%" -H "Content-Type: application/json" -d "@%HERE%book.json"
echo.

echo == Members: GET all ==
%CURL% -k -s "%BASE%/api/Members" -H "%AUTH%"
echo.

echo == Members: POST (create) ==
%CURL% -k -s -X POST "%BASE%/api/Members" -H "%AUTH%" -H "Content-Type: application/json" -d "@%HERE%member.json"
echo.

echo == Borrowings: POST /borrow (uses memberId=1, bookId=1) ==
%CURL% -k -s -X POST "%BASE%/api/Borrowings/borrow" -H "%AUTH%" -H "Content-Type: application/json" -d "@%HERE%borrow.json"
echo.

echo == Borrowings: GET all ==
%CURL% -k -s "%BASE%/api/Borrowings" -H "%AUTH%"
echo.

echo == Borrowings: POST /return (borrowingId=1) ==
%CURL% -k -s -X POST "%BASE%/api/Borrowings/return" -H "%AUTH%" -H "Content-Type: application/json" -d "@%HERE%return.json"
echo.

echo == Middleware header check (shows x-correlation-id) ==
%CURL% -k -i "%BASE%/api/Books" -H "%AUTH%" -H "X-Client-Id: dev" | findstr /I "x-correlation-id"
echo.

echo == Negative test: user cannot POST book (expect 403) ==
for /f "tokens=* usebackq" %%u in (`
  %CURL% -k -s -X POST "%BASE%/api/Auth/login" ^
    -H "Content-Type: application/json" ^
    -d "{ ""username"":""user"", ""password"":""user123"" }"
`) do set USERTOKEN=%%u
%CURL% -k -s -o NUL -w "HTTP %%{http_code}\n" -X POST "%BASE%/api/Books" -H "Authorization: Bearer %USERTOKEN%" -H "Content-Type: application/json" -d "@%HERE%book.json"

echo.
echo DONE ^
✅
pause
