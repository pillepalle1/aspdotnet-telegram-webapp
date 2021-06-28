SUBDIRS := $(wildcard */.)

.PHONY:	dev prod up down $(SUBDIRS)

rtfm: 
	$(error #rtfm: No build target provided)

start: up
stop: down

up:
	docker-compose up -d

down:
	docker-compose down --remove-orphans --rmi all

dev: clean
	ln -s docker-compose.development.yml docker-compose.yml
	docker-compose rm
	docker-compose build

prod: clean
	ln -s docker-compose.production.yml docker-compose.yml
	docker-compose rm
	docker-compose build

clean: $(SUBDIRS)
	rm -rf docker-compose.yml

$(SUBDIRS):
	$(MAKE) -C $@
