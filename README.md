## Getting Started

It is a N-Tier Architecture with Presentation, Domain and Infra layers.

At Presentation we have WebApi.VehiclesAuction.Api with methods for Admin that can create Auctions and add AuctionItems for these Auction. We have methods for Participant that can bid for these AuctionItems.

WebApi.VehiclesAuction.BackgroundServices has only two background methods: "AuditAuctions" that runs every hour, looking for Auctions that ended and setting a winner. And the "NotifyWinners" that run every day at 8am, looking for the winners to send a congratulations email by SendGrid. (You can trigger them manually at Hangfire dashboard)

You will need to create a account at SendGrid and update the credentials key on "EmailSender.cs" class.

## Running Project Locally

```bash
dotnet restore && dotnet build // On solutions folder
dotnet run // On each projects folder
```

## Running Database at Docker

```bash
// On docker's folder
docker build -t postgres-auction .
docker run -p 5434:5432 -d postgres-auction
```

## Running Database at pgAdmin

Create a database called "Auction" and change the "DefaultConnection" parameters if necessary

```bash
// On WebApi.VehiclesAuction.Infra
Update-Database
```
