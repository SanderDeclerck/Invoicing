echo "Installing the cosmos db dev cert"

curl -k --fail --retry-all-errors --retry 24 --retry-delay 5 --retry-max-time 120 https://db.local:8081/_explorer/emulator.pem > ~/emulatorcert.crt

sudo cp ~/emulatorcert.crt /usr/local/share/ca-certificates/
sudo update-ca-certificates
