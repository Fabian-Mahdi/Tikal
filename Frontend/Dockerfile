FROM node:24-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm ci

RUN npm install -g @angular/cli

COPY . .

RUN ng build --configuration=production

FROM nginx:1.29.0-alpine

COPY .nginx/nginx.conf /etc/nginx/conf.d/default.conf

COPY --from=build app/dist/Frontend/browser /var/www

EXPOSE 80