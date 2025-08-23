# Default target
.DEFAULT_GOAL := help

# Tools and projects
DOTNET := dotnet
EF := $(DOTNET) dotnet-ef
INFRA_PROJ := src/Learnify.VideoProcessing.Infrastructure.Persistence/Learnify.VideoProcessing.Infrastructure.Persistence.csproj
STARTUP_PROJ := src/Learnify.VideoProcessing.gRPC/Learnify.VideoProcessing.gRPC.csproj
MIGRATIONS_DIR := Migrations

.PHONY: help tool-restore build clean migrate-add migrate-remove migrate-update migrate-list migrate-script database-drop

help:
	@echo "Make targets for Learnify.VideoProcessing"
	@echo "-------------------------------------------------"
	@echo "make tool-restore               # Restore dotnet local tools (incl. dotnet-ef if defined)"
	@echo "make build                      # Build the startup project"
	@echo "make clean                      # Clean the solution/projects"
	@echo "make migrate-add name=XYZ       # Add a new migration named XYZ"
	@echo "make migrate-remove             # Remove the last migration"
	@echo "make migrate-update [to=XYZ]    # Update database to latest or to migration XYZ"
	@echo "make migrate-list               # List migrations"

# Restore local tools (if a tool manifest exists)
tool-restore:
	$(DOTNET) tool restore

build:
	$(DOTNET) build $(STARTUP_PROJ)

clean:
	$(DOTNET) clean

# Add a new migration: make migrate-add name=MyMigration
migrate-add:
ifeq ($(strip $(name)),)
	$(error Usage: make migrate-add name=YourMigrationName)
endif
	$(EF) migrations add $(name) \
		--project $(INFRA_PROJ) \
		--startup-project $(STARTUP_PROJ) \
		--output-dir $(MIGRATIONS_DIR)

# Remove last migration
migrate-remove:
	$(EF) migrations remove \
		--project $(INFRA_PROJ) \
		--startup-project $(STARTUP_PROJ)

# Update database to latest or a specific migration: make migrate-update or make migrate-update to=MigrationName
migrate-update:
ifeq ($(strip $(to)),)
	$(EF) database update \
		--project $(INFRA_PROJ) \
		--startup-project $(STARTUP_PROJ)
else
	$(EF) database update $(to) \
		--project $(INFRA_PROJ) \
		--startup-project $(STARTUP_PROJ)
endif

# List all migrations
migrate-list:
	$(EF) migrations list \
		--project $(INFRA_PROJ) \
		--startup-project $(STARTUP_PROJ)
