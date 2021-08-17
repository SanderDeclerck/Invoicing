# See here for image contents: https://github.com/microsoft/vscode-dev-containers/tree/v0.187.0/containers/ubuntu/.devcontainer/base.Dockerfile

# [Choice] Ubuntu version: bionic, focal
ARG VARIANT="focal"
FROM mcr.microsoft.com/vscode/devcontainers/base:0-${VARIANT}

# [Optional] Uncomment this section to install additional OS packages.
# RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
#     && apt-get -y install --no-install-recommends <your-package-list-here>


USER vscode
WORKDIR /installation

COPY scripts/*.sh /tmp/scripts/

RUN bash /tmp/scripts/install-node.sh
RUN bash /tmp/scripts/install-dotnet.sh
RUN curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
RUN curl -fsSL https://get.pulumi.com | bash

