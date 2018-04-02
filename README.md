# Easy-Rar-Unpacker

Welcome to my very first own Project on Github :)

So, you think, what´s this? I will explain why i make this program....

I´ve searched the internet for an easy RAR-Extractor. I found some programs, but some of them uses unrar.dll, some are to overloaded with functions...

So i´ve decided to wrote my own program. Because i´m a self-taught (Autodidact), and i wanted to test some new functions and test what i´ve learned
i´ve created this program. But i code only in vb.net, it was easier for me to learn :)

Okay, so why should you use this program? I will tell you:

Keyfeatures:

- Uses the fantastic SharpCompress Code from Adam Hathcock
- Very fast. All operations are done within threads (you can pause/unpause the extraction!) ThreadSafe communication with controls.
- Multilingual (german and english), own Language editor. You can EASILY translate it to your own language!
- Very easy to handle
- All changes (source / destination path and so on) are stored in a settings file (automatically save / load)

So i hope maybe you will use my little program and learn something from the code.


I think, now you want to know how you can extract archives with the program...

1. Click on "Add Path" to add one or more source folders to search for .rar archives. If you want to delete a folder, easily select the folder and click "Remove Path"
2. Select a Destination Path. If you checked "Same as Archive", all files will be extracted into the same path where the archives are and you can´t select a destination.
3. Click on "Scan Files". Every path will be scanned for .rar archives. ATM only rar archives will be extracted, sorry...
4. Check or uncheck "Extract Full Path" and/or "Overwrite". Extract Full Path means that the full path of the entry within the archive will be extracted, Overwrite means that an existing
file will be overwritten
5. Wait and look :)

And if you want to pause the extraction, for whatever reason, just click on "Pause" and click on "UnPause" to continue the extraction :)

-TODO:

- Update function
- Help text for buttons, controls, etc.

Nothin more, nothin less :)

Version History:

v1.00
- Initial Release