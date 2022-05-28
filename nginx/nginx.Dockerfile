FROM nginx

COPY nginx/nginx.local.conf /etc/nginx/nginx.conf

COPY nginx/id-local-hotelix.crt /etc/ssl/certs/id-local.hotelix.one.crt
COPY nginx/id-local-hotelix.key /etc/ssl/private/id-local.hotelix.one.key

COPY nginx/hotelix.crt /etc/ssl/certs/hotelix.one.crt
COPY nginx/hotelix.key /etc/ssl/private/hotelix.one.key