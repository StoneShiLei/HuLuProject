FROM node:14-alpine as build

WORKDIR /app
COPY . .
RUN npm install && npm run build

FROM nginx
RUN mkdir /app
COPY --from=build /app/dist /app
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80