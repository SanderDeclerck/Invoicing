# Do the npm install or yarn install in the full image
FROM mhart/alpine-node AS builder

ARG FONTAWESOME_NPM_AUTH_TOKEN

WORKDIR /app
COPY package.json .
COPY yarn.lock .
COPY .npmrc .

ENV FONTAWESOME_NPM_AUTH_TOKEN=${FONTAWESOME_NPM_AUTH_TOKEN}

RUN yarn install --frozen-lockfile
COPY . .
RUN yarn build && yarn --production

# And then copy over node_modules, etc from that stage to the smaller base image
FROM mhart/alpine-node:base
WORKDIR /app
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/.next ./.next
# COPY --from=builder /app/public ./public
EXPOSE 3000
CMD ["node_modules/.bin/next", "start"]
