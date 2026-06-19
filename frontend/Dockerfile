# Frontend React — Multi-stage build
FROM node:18-alpine AS build
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci

COPY . .
RUN npm run build

# Serveur nginx pour les fichiers statiques
FROM nginx:alpine AS runtime

# Configuration nginx pour SPA (fallback vers index.html)
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
