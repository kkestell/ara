FROM mcr.microsoft.com/dotnet/sdk:7.0

RUN apt-get update \
    && apt-get install -y \
    clang zlib1g-dev lsb-release wget software-properties-common gnupg \
    build-essential nodejs npm graphviz \
    --no-install-recommends \
    && rm -rf /var/lib/apt/lists/*

RUN bash -c "$(wget -O - https://apt.llvm.org/llvm.sh)"

# WORKDIR /ara/parser
# COPY parser/ ./
# WORKDIR /ara/parser/tree-sitter
# RUN make clean
# RUN make
# WORKDIR /ara/parser
# RUN npm install
# RUN make
# WORKDIR /ara/libara
# RUN make

# WORKDIR /ara/compiler
# COPY compiler/ ./
# RUN dotnet publish Ara -c release -r linux-x64 -o /ara/bin

RUN echo 'export PATH=$PATH:/ara/bin/' >> /root/.bashrc

ENV LLC=llc-15
ENV CLANG=clang-15

WORKDIR /ara/examples
