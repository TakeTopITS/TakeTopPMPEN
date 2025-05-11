rem restoring database..... 
set PGPASSWORD=zxckkllzly
echo on
..\pgsetup\postgresql\bin\pg_restore.exe -h 127.0.0.1 -p 5432 -U postgres -w -d taketopdecmpendb -v E:\TakeTopDECMPWinSolutionENCore\ENSource\Database\TakeTopAppDBBackup.bak
 rem restoring database sucessfully£¡ 
