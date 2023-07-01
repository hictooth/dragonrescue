# DragonRescue

## As of July 1st, 07:20 UTC, this no longer works as the School of Dragons servers have been switched off.

An export tool for your School of Dragons account data.

On 7th June, 2023, School of Dragons announced they were "sunsetting" the game, and turning the servers off on the 30th of June.

## Running

Either compile the tool yourself using the [instructions under Compiling below](#Compiling), or [download the latest release for your computer](https://github.com/hictooth/dragonrescue/releases/latest).

Then create a new empty folder for the export to be saved to

```
mkdir /tmp/sod-export
```

Finally run the export tool with your School of Dragons username and password:

```
./dragonrescue "username" "password" /tmp/sod-export
```

### Compiling

To compile, run the command:

```
dotnet publish -c Release --self-contained dragonrescue.sln
```

## Status

### What is exported

- account information
- profile list
- rank attributes
- inventory
- dragons
- dragon achievements
- user achievements
- quests
- gamedata
- buddies
- clans
- user activity
- active dragons
- rooms
- room items (I think this is eggs?)
- Images (of your dragons)
- Messages
- Subscription info

### What is not exported

Basically anything not on the above list, including but not limited to:

- GetValuePairs
- Announcements
