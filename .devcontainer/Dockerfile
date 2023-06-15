FROM archlinux:latest
USER root
RUN pacman -Syy --noconfirm
RUN pacman -Sy --noconfirm dotnet-sdk llvm clang nodejs npm make
RUN echo 'export PATH=$PATH:/ara/bin/' >> /root/.bashrc
