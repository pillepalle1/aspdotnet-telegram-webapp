# Telegram Webapp

This repository contains a net5.0 template for creating Webapps that feature a
*Telegram Bot* and *Telegram Based Authentication*. Official documentation can
be found here:

## Telegram Bot API

https://core.telegram.org/bots/api

## Telegram User Authentication

https://core.telegram.org/api/auth

## Installing the template

`cd` into the src/ directory and issue `dotnet new -i ./`

*Warning:* Do not remove the directory afterwards. You will run into a world of
pain becuase dotnet-new will keep looking for this template.

Restore the template by issuing `dotnet new <template-directory-name>`

Uninstall the template using `dotnet new -u`

---

# Third Party Libraries

This project heavily relies on third party libraries that implement interaction
with the Telegram servers.

## Telegram.Bot

Implementation of the Telegram Bot API with very helpful community on Telegram.

on Github: https://github.com/TelegramBots/Telegram.Bot

## Telegram.Bot.Extensions.LoginWidget

Implementation of the _Log in with Telegram_ button.

on Github: https://github.com/TelegramBots/Telegram.Bot.Extensions.LoginWidget

## Microsoft.EntityFrameworkCore.*

Well known mapper from dotnet core classes to database schemes.

## Npgsql.EntityFrameworkCore.PostgreSQL

Translates abstractions from Microsoft.EntityFrameworkCore to database
commands.
