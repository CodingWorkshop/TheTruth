cd .\nginx
start nginx

cd ..

dotnet publish .\TheTruth\TheTruth.csproj -c Release
cd .\TheTruth\bin\Release\PublishOutput\
dotnet TheTruth.dll