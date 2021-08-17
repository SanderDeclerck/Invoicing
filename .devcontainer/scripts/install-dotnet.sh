#!/bin/sh

mkdir -p $HOME/dotnet
curl -o $HOME/dotnet/dotnet-sdk.tar.gz https://download.visualstudio.microsoft.com/download/pr/ce3fd989-b69d-439a-9cac-09ad40597db8/2848d49480b6e7b1b2a18cfa46d724e2/dotnet-sdk-6.0.100-preview.7.21379.14-linux-x64.tar.gz

tar zxf $HOME/dotnet/dotnet-sdk.tar.gz -C $HOME/dotnet
rm -f $HOME/dotnet/dotnet-sdk.tar.gz

echo 'DOTNET_ROOT=$HOME/dotnet' >> $HOME/.bashrc
echo 'PATH=$PATH:$HOME/dotnet' >> $HOME/.bashrc