#!/bin/bash

CERT_STORE_BASE="/etc/letsencrypt/live"
CERT_STORE="${CERT_STORE_BASE}/${FQDN}"

while [[ true ]]
do

  if [[ -d "${CERT_STORE}" ]]
  then

    echo "Certificate store found. Attempting to renew ..."
    certbot renew

  else

    echo "Certificate store not found. Initializing ..."

    echo "  > Attempting to obtain certificates"
    certbot certonly \
        --verbose \
        --server https://acme-v02.api.letsencrypt.org/directory \
        --authenticator "standalone" \
        --agree-tos \
        --email "${EMAIL}" \
        -d "${FQDN}"

    echo "  > Creating symlink so the certificates can be found by nginx ..."
    ln -s "${FQDN}" "${CERT_STORE_BASE}/this"

  fi

  # Attempting to renew every 12 hours as suggested by letsencrypt
  sleep 12h

done
