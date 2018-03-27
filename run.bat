IF exist .\nginx\nginx.exe (
echo Find nginx.exe
) ELSE (
copy .\nginx\nginx.exe_ .\nginx\nginx.exe
)

cd .\nginx
start nginx

cd ..

dotnet publish .\TheTruth\TheTruth.csproj -c Release
cd .\TheTruth\bin\Release\PublishOutput\
dotnet TheTruth.dll