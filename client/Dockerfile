# Define node version
FROM node:16-alpine3.17 as build
# Define container directory
WORKDIR /usr/src/app
# Copy package*.json for npm install
COPY client/package*.json ./
# Run npm clean install, including dev dependencies for @angular-devkit
RUN npm ci
# Run npm install @angular/cli
RUN npm install -g @angular/cli
# Copy all files
COPY client .
# Run ng build through npm to create dist folder
RUN npm run build --prod
# Define nginx for front-end server
FROM nginx:1.21.1-alpine
# Copy dist from ng build to nginx html folder
COPY --from=build /usr/src/app/dist /usr/share/nginx/html
# Copy custom nginx.conf file with HTTPS settings
COPY client/nginx/nginx.conf /etc/nginx/nginx.conf

RUN mkdir /etc/nginx/certs
COPY client/nginx/certs/server.crt /etc/nginx/certs/server.crt
COPY client/nginx/certs/server.key /etc/nginx/certs/server.key

# Expose port 80 for HTTP and port 443 for HTTPS
EXPOSE 80
EXPOSE 443
# Start nginx server
CMD ["nginx", "-g", "daemon off;"]
