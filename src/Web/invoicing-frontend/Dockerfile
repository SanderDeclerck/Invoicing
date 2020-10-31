# Do the npm install or yarn install in the full image
FROM node:12 AS build

ARG FONTAWESOME_NPM_AUTH_TOKEN
ARG OIDC_AUTHORITY
ARG OIDC_CALLBACK
ARG OIDC_POST_LOGOUT_REDIRECT
ARG CUSTOMER_SERVICE_API_BASEURL

WORKDIR /app
COPY package.json .
COPY yarn.lock .
COPY .npmrc .

ENV FONTAWESOME_NPM_AUTH_TOKEN=${FONTAWESOME_NPM_AUTH_TOKEN}

RUN yarn install --frozen-lockfile

ENV OIDC_AUTHORITY=${OIDC_AUTHORITY}
ENV OIDC_CALLBACK=${OIDC_CALLBACK}
ENV OIDC_POST_LOGOUT_REDIRECT=${OIDC_POST_LOGOUT_REDIRECT}
ENV CUSTOMER_SERVICE_API_BASEURL=${CUSTOMER_SERVICE_API_BASEURL}

COPY src/ ./src
RUN yarn build

FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD [ "nginx", "-g", "daemon off;" ]