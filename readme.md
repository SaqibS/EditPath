# EditPath

This is a quick-and-dirty program I hacked together when I wanted to clean up the _path_ environment variable on a Windows machine.

The default way of doing this is cumbersome to find (Control Panel/System/Advanced/Environment Variables), and requires editing a long string.

So this tool (which you should "Run as Administrator") gives you all the directories in the list, making it easy to add/remove/reorder. It also lets you remove directories no longer on disk, and remove duplicates. The _path_ environment variable is saved upon exit.

The code is awful, but it served my purpose! :-)
