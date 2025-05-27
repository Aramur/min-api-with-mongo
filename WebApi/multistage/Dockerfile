FROM gcc:4.9 AS build
WORKDIR /app
COPY . .
RUN gcc -static -o hello hello.c

FROM scratch AS runtime
COPY --from=build /app/hello /hello
CMD ["/hello"]