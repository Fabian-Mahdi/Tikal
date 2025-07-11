FROM node:24-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm install

RUN npm install -g @angular/cli

COPY . .

RUN ng build --configuration=production

FROM nginx:1.29.0-alpine

COPY --from=build app/dist/Frontend/browser /usr/share/nginx/html

EXPOSE 80