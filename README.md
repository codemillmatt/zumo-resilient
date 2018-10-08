# Lessons Learned from Making Resilient Apps with Azure Mobile App Services

Users of mobile apps are just that, mobile. And as such, any mobile app that deals with data needs to work reliably in all environments; from a WiFi connection with a high throughput Internet connection, to an intermittent cell connection that keeps dropping - it even needs to work for prolonged periods of offline use. Not only that, when the device gets back online, your app needs to gracefully handle data synchronization and any data conflicts as well.

In this session, I will share with you hard earned lessons on how to solve the problem of online/offline data resiliency and synchronization using Azure Mobile App Services based on using it in multiple Xamarin projects. You will learn how to solve the issues that arise with improper client-side data schemas, pushing data to the server without knowing when the connection will be interrupted, and reconciling conflicting data between the server and the app. At the end of this session you'll be well equipped to have your app ready for anything.

## Slides

- [Here you go!](ResilientApps.pdf)

## Demos

### Demo 1 - Online/Offline Data

In this first demo we took a lap around what [Azure App Mobile Services](https://msou.co/bni) is both on the [server side](https://msou.co/bnj) and on the [client side](https://msou.co/bnk).

The service side is composed of "normal" controllers in the everyday sense, but they inherit from a [special class](https://msou.co/bnl): `TableController`.

There is a [NuGet package](https://msou.co/bnm) on the client side that performs both saving data locally and also sync'ing it up to the server. We looked at how everything is [saved to the client until it is explicitly told to sync to the server](https://msou.co/bnn).

Of course, before syncing to the server, always make sure you're connected to the internet!

### Demo 2 - Conflict resolution

When you make edits to [data while offline](https://msou.co/bno), it's inevitable that somebody else will edit the same data, and then when you go to sync you'll find that your data conflicts with the data on the server.

This demo showed the various ways to [deal with that on the client](https://msou.co/bnp).

You could decide to always have the version of the record on the server win. Or always have the record on the client win. Or, as you would in most cases, use some sort of logic to combine the values record on the server with those on the client and send that back up to be saved.

This is one of the rare cases where you want to manually change the contents of the [record's _Version_ property](https://msou.co/bnq), in order to be sure the changes do not get rejected again.

### Demo 3 - Silent push

Finally we took a look at how to keep the user's local database up to date with the server's, without the user even knowing about it, with a technique known as silent data pushing.

We used [App Center](https://msou.co/bnr) to do this.

App Center has a feature that allows you to send [push notifications](https://msou.co/bns) to mobile clients. Using a [cross platform SDK](https://msou.co/bnt), you can receive those push notifications, and then act upon them ... doing things such as initiating a data download!

## Check out these resources to learn even more about the topics we talked about!!

- [Azure Mobile App Services](https://msou.co/bnu)
- [Working with the Mobile App Service Client](https://msou.co/bnv)
- [Offline Mobile Sync](https://msou.co/bnw)
- [Azure App Center Push Notifications](https://msou.co/bns)
