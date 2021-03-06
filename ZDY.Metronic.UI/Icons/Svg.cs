﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ZDY.Metronic.UI.Icons
{
    internal static class Svg
    {                   
        internal static Dictionary<SvgIcon, string> All = new Dictionary<SvgIcon, string>
        {
            {SvgIcon.Brassiere, "Clothes/Brassiere.svg"},
            {SvgIcon.Briefcase, "Clothes/Briefcase.svg"},
            {SvgIcon.Cap, "Clothes/Cap.svg"},
            {SvgIcon.Crown, "Clothes/Crown.svg"},
            {SvgIcon.Dress, "Clothes/Dress.svg"},
            {SvgIcon.Hanger, "Clothes/Hanger.svg"},
            {SvgIcon.Hat, "Clothes/Hat.svg"},
            {SvgIcon.Panties, "Clothes/Panties.svg"},
            {SvgIcon.Shirt, "Clothes/Shirt.svg"},
            {SvgIcon.Shoes, "Clothes/Shoes.svg"},
            {SvgIcon.Shorts, "Clothes/Shorts.svg"},
            {SvgIcon.Sneakers, "Clothes/Sneakers.svg"},
            {SvgIcon.Socks, "Clothes/Socks.svg"},
            {SvgIcon.SunGlasses, "Clothes/Sun-glasses.svg"},
            {SvgIcon.TShirt, "Clothes/T-Shirt.svg"},
            {SvgIcon.Tie, "Clothes/Tie.svg"},
            {SvgIcon.Backspace, "Code/Backspace.svg"},
            {SvgIcon.CMD, "Code/CMD.svg"},
            {SvgIcon.Code, "Code/Code.svg"},
            {SvgIcon.Commit, "Code/Commit.svg"},
            {SvgIcon.Compiling, "Code/Compiling.svg"},
            {SvgIcon.Control, "Code/Control.svg"},
            {SvgIcon.DoneCircle, "Code/Done-circle.svg"},
            {SvgIcon.ErrorCircle, "Code/Error-circle.svg"},
            {SvgIcon.Git1, "Code/Git1.svg"},
            {SvgIcon.Git2, "Code/Git2.svg"},
            {SvgIcon.Git3, "Code/Git3.svg"},
            {SvgIcon.Git4, "Code/Git4.svg"},
            {SvgIcon.Github, "Code/Github.svg"},
            {SvgIcon.InfoCircle, "Code/Info-circle.svg"},
            {SvgIcon.LeftCircle, "Code/Left-circle.svg"},
            {SvgIcon.Loading, "Code/Loading.svg"},
            {SvgIcon.LockCircle, "Code/Lock-circle.svg"},
            {SvgIcon.LockOverturning, "Code/Lock-overturning.svg"},
            {SvgIcon.Minus, "Code/Minus.svg"},
            {SvgIcon.Option, "Code/Option.svg"},
            {SvgIcon.Plus, "Code/Plus.svg"},
            {SvgIcon.Puzzle, "Code/Puzzle.svg"},
            {SvgIcon.QuestionCircle, "Code/Question-circle.svg"},
            {SvgIcon.RightCircle, "Code/Right-circle.svg"},
            {SvgIcon.Settings4, "Code/Settings4.svg"},
            {SvgIcon.Shift, "Code/Shift.svg"},
            {SvgIcon.Spy, "Code/Spy.svg"},
            {SvgIcon.Stop, "Code/Stop.svg"},
            {SvgIcon.Terminal, "Code/Terminal.svg"},
            {SvgIcon.ThunderCircle, "Code/Thunder-circle.svg"},
            {SvgIcon.TimeSchedule, "Code/Time-schedule.svg"},
            {SvgIcon.Warning1Circle, "Code/Warning-1-circle.svg"},
            {SvgIcon.Warning2, "Code/Warning-2.svg"},
            {SvgIcon.ActiveCall, "Communication/Active-call.svg"},
            {SvgIcon.AddUser, "Communication/Add-user.svg"},
            {SvgIcon.AddressCard, "Communication/Address-card.svg"},
            {SvgIcon.AdressBook1, "Communication/Adress-book1.svg"},
            {SvgIcon.AdressBook2, "Communication/Adress-book2.svg"},
            {SvgIcon.Archive, "Communication/Archive.svg"},
            {SvgIcon.Call1, "Communication/Call1.svg"},
            {SvgIcon.Call, "Communication/Call.svg"},
            {SvgIcon.ChatCheck, "Communication/Chat-check.svg"},
            {SvgIcon.ChatError, "Communication/Chat-error.svg"},
            {SvgIcon.ChatLocked, "Communication/Chat-locked.svg"},
            {SvgIcon.ChatSmile, "Communication/Chat-smile.svg"},
            {SvgIcon.Chat1, "Communication/Chat1.svg"},
            {SvgIcon.Chat2, "Communication/Chat2.svg"},
            {SvgIcon.Chat4, "Communication/Chat4.svg"},
            {SvgIcon.Chat5, "Communication/Chat5.svg"},
            {SvgIcon.Chat6, "Communication/Chat6.svg"},
            {SvgIcon.ClipboardCheck, "Communication/Clipboard-check.svg"},
            {SvgIcon.ClipboardList, "Communication/Clipboard-list.svg"},
            {SvgIcon.Contact1, "Communication/Contact1.svg"},
            {SvgIcon.DeleteUser, "Communication/Delete-user.svg"},
            {SvgIcon.DialNumbers, "Communication/Dial-numbers.svg"},
            {SvgIcon.Flag, "Communication/Flag.svg"},
            {SvgIcon.Forward, "Communication/Forward.svg"},
            {SvgIcon.GroupChat, "Communication/Group-chat.svg"},
            {SvgIcon.Group, "Communication/Group.svg"},
            {SvgIcon.IncomingBox, "Communication/Incoming-box.svg"},
            {SvgIcon.IncomingCall, "Communication/Incoming-call.svg"},
            {SvgIcon.IncomingMail, "Communication/Incoming-mail.svg"},
            {SvgIcon.MailAt, "Communication/Mail-at.svg"},
            {SvgIcon.MailAttachment, "Communication/Mail-attachment.svg"},
            {SvgIcon.MailBox, "Communication/Mail-box.svg"},
            {SvgIcon.MailError, "Communication/Mail-error.svg"},
            {SvgIcon.MailHeart, "Communication/Mail-heart.svg"},
            {SvgIcon.MailLocked, "Communication/Mail-locked.svg"},
            {SvgIcon.MailNotification, "Communication/Mail-notification.svg"},
            {SvgIcon.MailOpened, "Communication/Mail-opened.svg"},
            {SvgIcon.MailUnocked, "Communication/Mail-unocked.svg"},
            {SvgIcon.Mail, "Communication/Mail.svg"},
            {SvgIcon.MissedCall, "Communication/Missed-call.svg"},
            {SvgIcon.OutgoingBox, "Communication/Outgoing-box.svg"},
            {SvgIcon.OutgoingCall, "Communication/Outgoing-call.svg"},
            {SvgIcon.OutgoingMail, "Communication/Outgoing-mail.svg"},
            {SvgIcon.ReadedMail, "Communication/Readed-mail.svg"},
            {SvgIcon.ReplyAll, "Communication/Reply-all.svg"},
            {SvgIcon.Reply, "Communication/Reply.svg"},
            {SvgIcon.Right, "Communication/Right.svg"},
            {SvgIcon.RSS, "Communication/RSS.svg"},
            {SvgIcon.SafeChat, "Communication/Safe-chat.svg"},
            {SvgIcon.Send, "Communication/Send.svg"},
            {SvgIcon.SendingMail, "Communication/Sending mail.svg"},
            {SvgIcon.Sending, "Communication/Sending.svg"},
            {SvgIcon.Share, "Communication/Share.svg"},
            {SvgIcon.ShieldThunder, "Communication/Shield-thunder.svg"},
            {SvgIcon.ShieldUser, "Communication/Shield-user.svg"},
            {SvgIcon.SnoozedMail, "Communication/Snoozed-mail.svg"},
            {SvgIcon.Spam, "Communication/Spam.svg"},
            {SvgIcon.Thumbtack, "Communication/Thumbtack.svg"},
            {SvgIcon.UrgentMail, "Communication/Urgent-mail.svg"},
            {SvgIcon.Write, "Communication/Write.svg"},
            {SvgIcon.BakingGlove, "Cooking/Baking-glove.svg"},
            {SvgIcon.Bowl, "Cooking/Bowl.svg"},
            {SvgIcon.Chef, "Cooking/Chef.svg"},
            {SvgIcon.CookingBook, "Cooking/Cooking-book.svg"},
            {SvgIcon.CookingPot, "Cooking/Cooking-pot.svg"},
            {SvgIcon.CuttingBoard, "Cooking/Cutting board.svg"},
            {SvgIcon.CookingDinner, "Cooking/Dinner.svg"},
            {SvgIcon.Dish, "Cooking/Dish.svg"},
            {SvgIcon.Dishes, "Cooking/Dishes.svg"},
            {SvgIcon.ForkSpoonKnife, "Cooking/Fork-spoon-knife.svg"},
            {SvgIcon.ForkSpoon, "Cooking/Fork-spoon.svg"},
            {SvgIcon.Fork, "Cooking/Fork.svg"},
            {SvgIcon.FryingPan, "Cooking/Frying-pan.svg"},
            {SvgIcon.Grater, "Cooking/Grater.svg"},
            {SvgIcon.KitchenScale, "Cooking/Kitchen-scale.svg"},
            {SvgIcon.Knife1, "Cooking/Knife1.svg"},
            {SvgIcon.Knife2, "Cooking/Knife2.svg"},
            {SvgIcon.KnifeAndFork1, "Cooking/KnifeAndFork1.svg"},
            {SvgIcon.KnifeAndFork2, "Cooking/KnifeAndFork2.svg"},
            {SvgIcon.Ladle, "Cooking/Ladle.svg"},
            {SvgIcon.RollingPin, "Cooking/Rolling-pin.svg"},
            {SvgIcon.Saucepan, "Cooking/Saucepan.svg"},
            {SvgIcon.CookingShovel, "Cooking/Shovel.svg"},
            {SvgIcon.Sieve, "Cooking/Sieve.svg"},
            {SvgIcon.Spoon, "Cooking/Spoon.svg"},
            {SvgIcon.Adjust, "Design/Adjust.svg"},
            {SvgIcon.AnchorCenterDown, "Design/Anchor-center-down.svg"},
            {SvgIcon.AnchorCenterUp, "Design/Anchor-center-up.svg"},
            {SvgIcon.AnchorCenter, "Design/Anchor-center.svg"},
            {SvgIcon.AnchorLeftDown, "Design/Anchor-left-down.svg"},
            {SvgIcon.AnchorLeftUp, "Design/Anchor-left-up.svg"},
            {SvgIcon.AnchorLeft, "Design/Anchor-left.svg"},
            {SvgIcon.AnchorRightDown, "Design/Anchor-right-down.svg"},
            {SvgIcon.AnchorRightUp, "Design/Anchor-right-up.svg"},
            {SvgIcon.AnchorRight, "Design/Anchor-right.svg"},
            {SvgIcon.Arrows, "Design/Arrows.svg"},
            {SvgIcon.BezierCurve, "Design/Bezier-curve.svg"},
            {SvgIcon.Border, "Design/Border.svg"},
            {SvgIcon.DesignBrush, "Design/Brush.svg"},
            {SvgIcon.DesignBucket, "Design/Bucket.svg"},
            {SvgIcon.Cap1, "Design/Cap-1.svg"},
            {SvgIcon.Cap2, "Design/Cap-2.svg"},
            {SvgIcon.Cap3, "Design/Cap-3.svg"},
            {SvgIcon.Circle, "Design/Circle.svg"},
            {SvgIcon.ColorProfile, "Design/Color-profile.svg"},
            {SvgIcon.Color, "Design/Color.svg"},
            {SvgIcon.Component, "Design/Component.svg"},
            {SvgIcon.Crop, "Design/Crop.svg"},
            {SvgIcon.Difference, "Design/Difference.svg"},
            {SvgIcon.DesignEdit, "Design/Edit.svg"},
            {SvgIcon.Eraser, "Design/Eraser.svg"},
            {SvgIcon.Flatten, "Design/Flatten.svg"},
            {SvgIcon.FlipHorizontal, "Design/Flip-horizontal.svg"},
            {SvgIcon.FlipVertical, "Design/Flip-vertical.svg"},
            {SvgIcon.Horizontal, "Design/Horizontal.svg"},
            {SvgIcon.Image, "Design/Image.svg"},
            {SvgIcon.Interselect, "Design/Interselect.svg"},
            {SvgIcon.Join1, "Design/Join-1.svg"},
            {SvgIcon.Join2, "Design/Join-2.svg"},
            {SvgIcon.Join3, "Design/Join-3.svg"},
            {SvgIcon.Layers, "Design/Layers.svg"},
            {SvgIcon.Line, "Design/Line.svg"},
            {SvgIcon.Magic, "Design/Magic.svg"},
            {SvgIcon.Mask, "Design/Mask.svg"},
            {SvgIcon.Patch, "Design/Patch.svg"},
            {SvgIcon.PenToolVector, "Design/Pen-tool-vector.svg"},
            {SvgIcon.PenAndRuller, "Design/PenAndRuller.svg"},
            {SvgIcon.Pencil, "Design/Pencil.svg"},
            {SvgIcon.Picker, "Design/Picker.svg"},
            {SvgIcon.Pixels, "Design/Pixels.svg"},
            {SvgIcon.Polygon, "Design/Polygon.svg"},
            {SvgIcon.Position, "Design/Position.svg"},
            {SvgIcon.Rectangle, "Design/Rectangle.svg"},
            {SvgIcon.Saturation, "Design/Saturation.svg"},
            {SvgIcon.Select, "Design/Select.svg"},
            {SvgIcon.Sketch, "Design/Sketch.svg"},
            {SvgIcon.Stamp, "Design/Stamp.svg"},
            {SvgIcon.Substract, "Design/Substract.svg"},
            {SvgIcon.Target, "Design/Target.svg"},
            {SvgIcon.Triangle, "Design/Triangle.svg"},
            {SvgIcon.Union, "Design/Union.svg"},
            {SvgIcon.Vertical, "Design/Vertical.svg"},
            {SvgIcon.ZoomMinus, "Design/ZoomMinus.svg"},
            {SvgIcon.ZoomPlus, "Design/ZoomPlus.svg"},
            {SvgIcon.Airpods, "Devices/Airpods.svg"},
            {SvgIcon.Android, "Devices/Android.svg"},
            {SvgIcon.AppleWatch, "Devices/Apple-Watch.svg"},
            {SvgIcon.BatteryCharging, "Devices/Battery-charging.svg"},
            {SvgIcon.BatteryEmpty, "Devices/Battery-empty.svg"},
            {SvgIcon.BatteryFull, "Devices/Battery-full.svg"},
            {SvgIcon.BatteryHalf, "Devices/Battery-half.svg"},
            {SvgIcon.Bluetooth, "Devices/Bluetooth.svg"},
            {SvgIcon.Camera, "Devices/Camera.svg"},
            {SvgIcon.CardboardVr, "Devices/Cardboard-vr.svg"},
            {SvgIcon.Cassete, "Devices/Cassete.svg"},
            {SvgIcon.CPU1, "Devices/CPU1.svg"},
            {SvgIcon.CPU2, "Devices/CPU2.svg"},
            {SvgIcon.Diagnostics, "Devices/Diagnostics.svg"},
            {SvgIcon.Display1, "Devices/Display1.svg"},
            {SvgIcon.Display2, "Devices/Display2.svg"},
            {SvgIcon.Display3, "Devices/Display3.svg"},
            {SvgIcon.Gameboy, "Devices/Gameboy.svg"},
            {SvgIcon.Gamepad1, "Devices/Gamepad1.svg"},
            {SvgIcon.Gamepad2, "Devices/Gamepad2.svg"},
            {SvgIcon.Generator, "Devices/Generator.svg"},
            {SvgIcon.HardDrive, "Devices/Hard-drive.svg"},
            {SvgIcon.Headphones, "Devices/Headphones.svg"},
            {SvgIcon.Homepod, "Devices/Homepod.svg"},
            {SvgIcon.iMac, "Devices/iMac.svg"},
            {SvgIcon.iPhoneBack, "Devices/iPhone-back.svg"},
            {SvgIcon.iPhoneXBack, "Devices/iPhone-x-back.svg"},
            {SvgIcon.iPhoneX, "Devices/iPhone-X.svg"},
            {SvgIcon.Keyboard, "Devices/Keyboard.svg"},
            {SvgIcon.LaptopMacbook, "Devices/Laptop-macbook.svg"},
            {SvgIcon.Laptop, "Devices/Laptop.svg"},
            {SvgIcon.LTE1, "Devices/LTE1.svg"},
            {SvgIcon.LTE2, "Devices/LTE2.svg"},
            {SvgIcon.Mic, "Devices/Mic.svg"},
            {SvgIcon.Midi, "Devices/Midi.svg"},
            {SvgIcon.Mouse, "Devices/Mouse.svg"},
            {SvgIcon.Phone, "Devices/Phone.svg"},
            {SvgIcon.Printer, "Devices/Printer.svg"},
            {SvgIcon.Radio, "Devices/Radio.svg"},
            {SvgIcon.Router1, "Devices/Router1.svg"},
            {SvgIcon.Router2, "Devices/Router2.svg"},
            {SvgIcon.SDCard, "Devices/SD-card.svg"},
            {SvgIcon.Server, "Devices/Server.svg"},
            {SvgIcon.Speaker, "Devices/Speaker.svg"},
            {SvgIcon.Tablet, "Devices/Tablet.svg"},
            {SvgIcon.TV1, "Devices/TV1.svg"},
            {SvgIcon.TV2, "Devices/TV2.svg"},
            {SvgIcon.UsbStorage, "Devices/Usb-storage.svg"},
            {SvgIcon.USB, "Devices/USB.svg"},
            {SvgIcon.VideoCamera, "Devices/Video-camera.svg"},
            {SvgIcon.Watch1, "Devices/Watch1.svg"},
            {SvgIcon.Watch2, "Devices/Watch2.svg"},
            {SvgIcon.WiFi, "Devices/Wi-fi.svg"},
            {SvgIcon.AirConditioning, "Electric/Air-conditioning.svg"},
            {SvgIcon.airDryer, "Electric/air-dryer.svg"},
            {SvgIcon.Blender, "Electric/Blender.svg"},
            {SvgIcon.Fan, "Electric/Fan.svg"},
            {SvgIcon.Fridge, "Electric/Fridge.svg"},
            {SvgIcon.GasStove, "Electric/Gas-stove.svg"},
            {SvgIcon.Highvoltage, "Electric/Highvoltage.svg"},
            {SvgIcon.Iron, "Electric/Iron.svg"},
            {SvgIcon.Kettle, "Electric/Kettle.svg"},
            {SvgIcon.Mixer, "Electric/Mixer.svg"},
            {SvgIcon.Outlet, "Electric/Outlet.svg"},
            {SvgIcon.RangeHood, "Electric/Range-hood.svg"},
            {SvgIcon.Shutdown, "Electric/Shutdown.svg"},
            {SvgIcon.SocketEu, "Electric/Socket-eu.svg"},
            {SvgIcon.SocketUs, "Electric/Socket-us.svg"},
            {SvgIcon.Washer, "Electric/Washer.svg"},
            {SvgIcon.CloudDownload, "Files/Cloud-download.svg"},
            {SvgIcon.CloudUpload, "Files/Cloud-upload.svg"},
            {SvgIcon.Compilation, "Files/Compilation.svg"},
            {SvgIcon.CompiledFile, "Files/Compiled-file.svg"},
            {SvgIcon.DeletedFile, "Files/Deleted-file.svg"},
            {SvgIcon.DeletedFolder, "Files/Deleted-folder.svg"},
            {SvgIcon.Download, "Files/Download.svg"},
            {SvgIcon.DownloadedFile, "Files/DownloadedFile.svg"},
            {SvgIcon.DownloadsFolder, "Files/Downloads-folder.svg"},
            {SvgIcon.Export, "Files/Export.svg"},
            {SvgIcon.FileCloud, "Files/File-cloud.svg"},
            {SvgIcon.FileDone, "Files/File-done.svg"},
            {SvgIcon.FileMinus, "Files/File-minus.svg"},
            {SvgIcon.FilePlus, "Files/File-plus.svg"},
            {SvgIcon.File, "Files/File.svg"},
            {SvgIcon.FolderCheck, "Files/Folder-check.svg"},
            {SvgIcon.FolderCloud, "Files/Folder-cloud.svg"},
            {SvgIcon.FolderError, "Files/Folder-error.svg"},
            {SvgIcon.FolderHeart, "Files/Folder-heart.svg"},
            {SvgIcon.FolderMinus, "Files/Folder-minus.svg"},
            {SvgIcon.FolderPlus, "Files/Folder-plus.svg"},
            {SvgIcon.FolderSolid, "Files/Folder-solid.svg"},
            {SvgIcon.FolderStar, "Files/Folder-star.svg"},
            {SvgIcon.FolderThunder, "Files/Folder-thunder.svg"},
            {SvgIcon.FileFolder, "Files/Folder.svg"},
            {SvgIcon.GroupFolders, "Files/Group-folders.svg"},
            {SvgIcon.Import, "Files/Import.svg"},
            {SvgIcon.LockedFolder, "Files/Locked-folder.svg"},
            {SvgIcon.MediaFolder, "Files/Media-folder.svg"},
            {SvgIcon.MediaFile, "Files/Media.svg"},
            {SvgIcon.MusicFile, "Files/Music.svg"},
            {SvgIcon.Pictures1, "Files/Pictures1.svg"},
            {SvgIcon.Pictures2, "Files/Pictures2.svg"},
            {SvgIcon.ProtectedFile, "Files/Protected-file.svg"},
            {SvgIcon.SelectedFile, "Files/Selected-file.svg"},
            {SvgIcon.ShareFile, "Files/Share.svg"},
            {SvgIcon.UploadFolder, "Files/Upload-folder.svg"},
            {SvgIcon.Upload, "Files/Upload.svg"},
            {SvgIcon.UploadedFile, "Files/Uploaded-file.svg"},
            {SvgIcon.UserFolder, "Files/User-folder.svg"},
            {SvgIcon.Beer, "Food/Beer.svg"},
            {SvgIcon.Bottle1, "Food/Bottle1.svg"},
            {SvgIcon.Bottle2, "Food/Bottle2.svg"},
            {SvgIcon.Bread, "Food/Bread.svg"},
            {SvgIcon.Bucket, "Food/Bucket.svg"},
            {SvgIcon.Burger, "Food/Burger.svg"},
            {SvgIcon.Cake, "Food/Cake.svg"},
            {SvgIcon.Carrot, "Food/Carrot.svg"},
            {SvgIcon.Cheese, "Food/Cheese.svg"},
            {SvgIcon.Chicken, "Food/Chicken.svg"},
            {SvgIcon.Coffee1, "Food/Coffee1.svg"},
            {SvgIcon.Coffee2, "Food/Coffee2.svg"},
            {SvgIcon.Cookie, "Food/Cookie.svg"},
            {SvgIcon.Dinner, "Food/Dinner.svg"},
            {SvgIcon.Fish, "Food/Fish.svg"},
            {SvgIcon.FrenchBread, "Food/French Bread.svg"},
            {SvgIcon.GlassMartini, "Food/Glass-martini.svg"},
            {SvgIcon.IceCream1, "Food/Ice-cream1.svg"},
            {SvgIcon.IceCream2, "Food/Ice-cream2.svg"},
            {SvgIcon.MisoSoup, "Food/Miso-soup.svg"},
            {SvgIcon.Orange, "Food/Orange.svg"},
            {SvgIcon.Pizza, "Food/Pizza.svg"},
            {SvgIcon.Sushi, "Food/Sushi.svg"},
            {SvgIcon.TwoBottles, "Food/Two-bottles.svg"},
            {SvgIcon.Wine, "Food/Wine.svg"},
            {SvgIcon.Attachment1, "General/Attachment1.svg"},
            {SvgIcon.Attachment2, "General/Attachment2.svg"},
            {SvgIcon.Binocular, "General/Binocular.svg"},
            {SvgIcon.Bookmark, "General/Bookmark.svg"},
            {SvgIcon.Clip, "General/Clip.svg"},
            {SvgIcon.Clipboard, "General/Clipboard.svg"},
            {SvgIcon.Cursor, "General/Cursor.svg"},
            {SvgIcon.Dislike, "General/Dislike.svg"},
            {SvgIcon.Duplicate, "General/Duplicate.svg"},
            {SvgIcon.Edit, "General/Edit.svg"},
            {SvgIcon.ExpandArrows, "General/Expand-arrows.svg"},
            {SvgIcon.Fire, "General/Fire.svg"},
            {SvgIcon.Folder, "General/Folder.svg"},
            {SvgIcon.HalfHeart, "General/Half-heart.svg"},
            {SvgIcon.HalfStar, "General/Half-star.svg"},
            {SvgIcon.Heart, "General/Heart.svg"},
            {SvgIcon.Hidden, "General/Hidden.svg"},
            {SvgIcon.Like, "General/Like.svg"},
            {SvgIcon.Lock, "General/Lock.svg"},
            {SvgIcon.Notification2, "General/Notification2.svg"},
            {SvgIcon.Notifications1, "General/Notifications1.svg"},
            {SvgIcon.Other1, "General/Other1.svg"},
            {SvgIcon.Other2, "General/Other2.svg"},
            {SvgIcon.Sad, "General/Sad.svg"},
            {SvgIcon.Save, "General/Save.svg"},
            {SvgIcon.Scale, "General/Scale.svg"},
            {SvgIcon.Scissors, "General/Scissors.svg"},
            {SvgIcon.Search, "General/Search.svg"},
            {SvgIcon.Settings3, "General/Settings-3.svg"},
            {SvgIcon.Settings1, "General/Settings-1.svg"},
            {SvgIcon.Settings2, "General/Settings-2.svg"},
            {SvgIcon.ShieldCheck, "General/Shield-check.svg"},
            {SvgIcon.ShieldDisabled, "General/Shield-disabled.svg"},
            {SvgIcon.ShieldProtected, "General/Shield-protected.svg"},
            {SvgIcon.Size, "General/Size.svg"},
            {SvgIcon.Smile, "General/Smile.svg"},
            {SvgIcon.Star, "General/Star.svg"},
            {SvgIcon.ThunderMove, "General/Thunder-move.svg"},
            {SvgIcon.Thunder, "General/Thunder.svg"},
            {SvgIcon.Trash, "General/Trash.svg"},
            {SvgIcon.Unlock, "General/Unlock.svg"},
            {SvgIcon.Update, "General/Update.svg"},
            {SvgIcon.User, "General/User.svg"},
            {SvgIcon.Visible, "General/Visible.svg"},
            {SvgIcon.AirBallon, "Home/Air-ballon.svg"},
            {SvgIcon.AlarmClock, "Home/Alarm-clock.svg"},
            {SvgIcon.Armchair, "Home/Armchair.svg"},
            {SvgIcon.BagChair, "Home/Bag-chair.svg"},
            {SvgIcon.Bath, "Home/Bath.svg"},
            {SvgIcon.Bed, "Home/Bed.svg"},
            {SvgIcon.BookOpen, "Home/Book-open.svg"},
            {SvgIcon.Book, "Home/Book.svg"},
            {SvgIcon.Box, "Home/Box.svg"},
            {SvgIcon.Broom, "Home/Broom.svg"},
            {SvgIcon.Building, "Home/Building.svg"},
            {SvgIcon.Bulb1, "Home/Bulb1.svg"},
            {SvgIcon.Bulb2, "Home/Bulb2.svg"},
            {SvgIcon.Chair1, "Home/Chair1.svg"},
            {SvgIcon.Chair2, "Home/Chair2.svg"},
            {SvgIcon.Clock, "Home/Clock.svg"},
            {SvgIcon.Commode1, "Home/Commode1.svg"},
            {SvgIcon.Commode2, "Home/Commode2.svg"},
            {SvgIcon.Couch, "Home/Couch.svg"},
            {SvgIcon.Cupboard, "Home/Cupboard.svg"},
            {SvgIcon.Curtains, "Home/Curtains.svg"},
            {SvgIcon.Deer, "Home/Deer.svg"},
            {SvgIcon.DoorOpen, "Home/Door-open.svg"},
            {SvgIcon.Earth, "Home/Earth.svg"},
            {SvgIcon.Fireplace, "Home/Fireplace.svg"},
            {SvgIcon.Flashlight, "Home/Flashlight.svg"},
            {SvgIcon.Flower1, "Home/Flower1.svg"},
            {SvgIcon.Flower2, "Home/Flower2.svg"},
            {SvgIcon.Flower3, "Home/Flower3.svg"},
            {SvgIcon.Globe, "Home/Globe.svg"},
            {SvgIcon.HomeHeart, "Home/Home-heart.svg"},
            {SvgIcon.Home, "Home/Home.svg"},
            {SvgIcon.Key, "Home/Key.svg"},
            {SvgIcon.Ladder, "Home/Ladder.svg"},
            {SvgIcon.Lamp1, "Home/Lamp1.svg"},
            {SvgIcon.Lamp2, "Home/Lamp2.svg"},
            {SvgIcon.Library, "Home/Library.svg"},
            {SvgIcon.Mailbox, "Home/Mailbox.svg"},
            {SvgIcon.Mirror, "Home/Mirror.svg"},
            {SvgIcon.Picture, "Home/Picture.svg"},
            {SvgIcon.Ruller, "Home/Ruller.svg"},
            {SvgIcon.Stairs, "Home/Stairs.svg"},
            {SvgIcon.Timer, "Home/Timer.svg"},
            {SvgIcon.Toilet, "Home/Toilet.svg"},
            {SvgIcon.Towel, "Home/Towel.svg"},
            {SvgIcon.HomeTrash, "Home/Trash.svg"},
            {SvgIcon.WaterMixer, "Home/Water-mixer.svg"},
            {SvgIcon.Weight1, "Home/Weight1.svg"},
            {SvgIcon.Weight2, "Home/Weight2.svg"},
            {SvgIcon.WoodHorse, "Home/Wood-horse.svg"},
            {SvgIcon.Wood1, "Home/Wood1.svg"},
            {SvgIcon.Wood2, "Home/Wood2.svg"},
            {SvgIcon.Layout3d, "Layout/Layout-3d.svg"},
            {SvgIcon.Layout4Blocks, "Layout/Layout-4-blocks.svg"},
            {SvgIcon.LayoutArrange, "Layout/Layout-arrange.svg"},
            {SvgIcon.LayoutGrid, "Layout/Layout-grid.svg"},
            {SvgIcon.LayoutHorizontal, "Layout/Layout-horizontal.svg"},
            {SvgIcon.LayoutLeftPanel1, "Layout/Layout-left-panel-1.svg"},
            {SvgIcon.LayoutLeftPanel2, "Layout/Layout-left-panel-2.svg"},
            {SvgIcon.LayoutRightPanel1, "Layout/Layout-right-panel-1.svg"},
            {SvgIcon.LayoutRightPanel2, "Layout/Layout-right-panel-2.svg"},
            {SvgIcon.LayoutTopPanel1, "Layout/Layout-top-panel-1.svg"},
            {SvgIcon.LayoutTopPanel2, "Layout/Layout-top-panel-2.svg"},
            {SvgIcon.LayoutTopPanel3, "Layout/Layout-top-panel-3.svg"},
            {SvgIcon.LayoutTopPanel4, "Layout/Layout-top-panel-4.svg"},
            {SvgIcon.LayoutTopPanel5, "Layout/Layout-top-panel-5.svg"},
            {SvgIcon.LayoutTopPanel6, "Layout/Layout-top-panel-6.svg"},
            {SvgIcon.LayoutVertical, "Layout/Layout-vertical.svg"},
            {SvgIcon.MapCompass, "Map/Compass.svg"},
            {SvgIcon.Direction1, "Map/Direction1.svg"},
            {SvgIcon.Direction2, "Map/Direction2.svg"},
            {SvgIcon.LocationArrow, "Map/Location-arrow.svg"},
            {SvgIcon.Marker1, "Map/Marker1.svg"},
            {SvgIcon.Marker2, "Map/Marker2.svg"},
            {SvgIcon.MapPosition, "Map/Position.svg"},
            {SvgIcon.AddMusic, "Media/Add-music.svg"},
            {SvgIcon.AirplayVideo, "Media/Airplay-video.svg"},
            {SvgIcon.Airplay, "Media/Airplay.svg"},
            {SvgIcon.Back, "Media/Back.svg"},
            {SvgIcon.Backward, "Media/Backward.svg"},
            {SvgIcon.CD, "Media/CD.svg"},
            {SvgIcon.DVD, "Media/DVD.svg"},
            {SvgIcon.Eject, "Media/Eject.svg"},
            {SvgIcon.Equalizer, "Media/Equalizer.svg"},
            {SvgIcon.MediaForward, "Media/Forward.svg"},
            {SvgIcon.MediaLibrary1, "Media/Media-library1.svg"},
            {SvgIcon.MediaLibrary2, "Media/Media-library2.svg"},
            {SvgIcon.MediaLibrary3, "Media/Media-library3.svg"},
            {SvgIcon.MovieLane1, "Media/Movie-lane1.svg"},
            {SvgIcon.MovieLane2, "Media/Movie-Lane2.svg"},
            {SvgIcon.MusicCloud, "Media/Music-cloud.svg"},
            {SvgIcon.MusicNote, "Media/Music-note.svg"},
            {SvgIcon.Music, "Media/Music.svg"},
            {SvgIcon.Mute, "Media/Mute.svg"},
            {SvgIcon.Next, "Media/Next.svg"},
            {SvgIcon.Pause, "Media/Pause.svg"},
            {SvgIcon.Play, "Media/Play.svg"},
            {SvgIcon.Playlist1, "Media/Playlist1.svg"},
            {SvgIcon.Playlist2, "Media/Playlist2.svg"},
            {SvgIcon.Rec, "Media/Rec.svg"},
            {SvgIcon.RepeatOne, "Media/Repeat-one.svg"},
            {SvgIcon.Repeat, "Media/Repeat.svg"},
            {SvgIcon.Shuffle, "Media/Shuffle.svg"},
            {SvgIcon.VolumeDown, "Media/Volume-down.svg"},
            {SvgIcon.VolumeFull, "Media/Volume-full.svg"},
            {SvgIcon.VolumeHalf, "Media/Volume-half.svg"},
            {SvgIcon.VolumeUp, "Media/Volume-up.svg"},
            {SvgIcon.Vynil, "Media/Vynil.svg"},
            {SvgIcon.Youtube, "Media/Youtube.svg"},
            {SvgIcon.NavigationAngleDoubleDown, "Navigation/Angle-double-down.svg"},
            {SvgIcon.NavigationAngleDoubleLeft, "Navigation/Angle-double-left.svg"},
            {SvgIcon.NavigationAngleDoubleRight, "Navigation/Angle-double-right.svg"},
            {SvgIcon.NavigationAngleDoubleUp, "Navigation/Angle-double-up.svg"},
            {SvgIcon.NavigationAngleDown, "Navigation/Angle-down.svg"},
            {SvgIcon.NavigationAngleLeft, "Navigation/Angle-left.svg"},
            {SvgIcon.NavigationAngleRight, "Navigation/Angle-right.svg"},
            {SvgIcon.NavigationArrowDown, "Navigation/Arrow-down.svg"},
            {SvgIcon.NavigationArrowFromBottom, "Navigation/Arrow-from-bottom.svg"},
            {SvgIcon.NavigationArrowFromLeft, "Navigation/Arrow-from-left.svg"},
            {SvgIcon.NavigationArrowFromRight, "Navigation/Arrow-from-right.svg"},
            {SvgIcon.NavigationArrowFromTop, "Navigation/Arrow-from-top.svg"},
            {SvgIcon.NavigationArrowLeft, "Navigation/Arrow-left.svg"},
            {SvgIcon.NavigationArrowRight, "Navigation/Arrow-right.svg"},
            {SvgIcon.NavigationArrowToBottom, "Navigation/Arrow-to-bottom.svg"},
            {SvgIcon.NavigationArrowToLeft, "Navigation/Arrow-to-left.svg"},
            {SvgIcon.NavigationArrowToRight, "Navigation/Arrow-to-right.svg"},
            {SvgIcon.NavigationArrowToUp, "Navigation/Arrow-to-up.svg"},
            {SvgIcon.NavigationArrowUp, "Navigation/Arrow-up.svg"},
            {SvgIcon.NavigationArrowsH, "Navigation/Arrows-h.svg"},
            {SvgIcon.NavigationArrowsV, "Navigation/Arrows-v.svg"},
            {SvgIcon.NavigationCheck, "Navigation/Check.svg"},
            {SvgIcon.NavigationClose, "Navigation/Close.svg"},
            {SvgIcon.NavigationDoubleCheck, "Navigation/Double-check.svg"},
            {SvgIcon.NavigationDown2, "Navigation/Down-2.svg"},
            {SvgIcon.NavigationDownLeft, "Navigation/Down-left.svg"},
            {SvgIcon.NavigationDownRight, "Navigation/Down-right.svg"},
            {SvgIcon.NavigationExchange, "Navigation/Exchange.svg"},
            {SvgIcon.NavigationLeft3, "Navigation/Left 3.svg"},
            {SvgIcon.NavigationLeft2, "Navigation/Left-2.svg"},
            {SvgIcon.NavigationMinus, "Navigation/Minus.svg"},
            {SvgIcon.NavigationPlus, "Navigation/Plus.svg"},
            {SvgIcon.NavigationRight3, "Navigation/Right 3.svg"},
            {SvgIcon.NavigationRight2, "Navigation/Right-2.svg"},
            {SvgIcon.NavigationRoute, "Navigation/Route.svg"},
            {SvgIcon.NavigationSignIn, "Navigation/Sign-in.svg"},
            {SvgIcon.NavigationSignOut, "Navigation/Sign-out.svg"},
            {SvgIcon.NavigationAngleUp, "Navigation/Angle-up.svg"},
            {SvgIcon.NavigationUp2, "Navigation/Up-2.svg"},
            {SvgIcon.NavigationUpDown, "Navigation/Up-down.svg"},
            {SvgIcon.NavigationUpLeft, "Navigation/Up-left.svg"},
            {SvgIcon.NavigationUpRight, "Navigation/Up-right.svg"},
            {SvgIcon.NavigationWaiting, "Navigation/Waiting.svg"},
            {SvgIcon.ATM, "Shopping/ATM.svg"},
            {SvgIcon.Bag1, "Shopping/Bag1.svg"},
            {SvgIcon.Bag2, "Shopping/Bag2.svg"},
            {SvgIcon.BarcodeRead, "Shopping/Barcode-read.svg"},
            {SvgIcon.BarcodeScan, "Shopping/Barcode-scan.svg"},
            {SvgIcon.Barcode, "Shopping/Barcode.svg"},
            {SvgIcon.Bitcoin, "Shopping/Bitcoin.svg"},
            {SvgIcon.Box1, "Shopping/Box1.svg"},
            {SvgIcon.Box2, "Shopping/Box2.svg"},
            {SvgIcon.Box3, "Shopping/Box3.svg"},
            {SvgIcon.Calculator, "Shopping/Calculator.svg"},
            {SvgIcon.Cart1, "Shopping/Cart1.svg"},
            {SvgIcon.Cart2, "Shopping/Cart2.svg"},
            {SvgIcon.Cart3, "Shopping/Cart3.svg"},
            {SvgIcon.ChartBar1, "Shopping/Chart-bar1.svg"},
            {SvgIcon.ChartBar2, "Shopping/Chart-bar2.svg"},
            {SvgIcon.ChartBar3, "Shopping/Chart-bar3.svg"},
            {SvgIcon.ChartLine1, "Shopping/Chart-line1.svg"},
            {SvgIcon.ChartLine2, "Shopping/Chart-line2.svg"},
            {SvgIcon.ChartPie, "Shopping/Chart-pie.svg"},
            {SvgIcon.CreditCard, "Shopping/Credit-card.svg"},
            {SvgIcon.Dollar, "Shopping/Dollar.svg"},
            {SvgIcon.Euro, "Shopping/Euro.svg"},
            {SvgIcon.Gift, "Shopping/Gift.svg"},
            {SvgIcon.Loader, "Shopping/Loader.svg"},
            {SvgIcon.MC, "Shopping/MC.svg"},
            {SvgIcon.Money, "Shopping/Money.svg"},
            {SvgIcon.Pound, "Shopping/Pound.svg"},
            {SvgIcon.Price1, "Shopping/Price1.svg"},
            {SvgIcon.Price2, "Shopping/Price2.svg"},
            {SvgIcon.Rouble, "Shopping/Rouble.svg"},
            {SvgIcon.Safe, "Shopping/Safe.svg"},
            {SvgIcon.Sale1, "Shopping/Sale1.svg"},
            {SvgIcon.Sale2, "Shopping/Sale2.svg"},
            {SvgIcon.Settings, "Shopping/Settings.svg"},
            {SvgIcon.Sort1, "Shopping/Sort1.svg"},
            {SvgIcon.Sort2, "Shopping/Sort2.svg"},
            {SvgIcon.Sort3, "Shopping/Sort3.svg"},
            {SvgIcon.Ticket, "Shopping/Ticket.svg"},
            {SvgIcon.Wallet, "Shopping/Wallet.svg"},
            {SvgIcon.Wallet2, "Shopping/Wallet2.svg"},
            {SvgIcon.Wallet3, "Shopping/Wallet3.svg"},
            {SvgIcon.AlignAuto, "Text/Align-auto.svg"},
            {SvgIcon.AlignCenter, "Text/Align-center.svg"},
            {SvgIcon.AlignJustify, "Text/Align-justify.svg"},
            {SvgIcon.AlignLeft, "Text/Align-left.svg"},
            {SvgIcon.AlignRight, "Text/Align-right.svg"},
            {SvgIcon.Article, "Text/Article.svg"},
            {SvgIcon.Bold, "Text/Bold.svg"},
            {SvgIcon.BulletList, "Text/Bullet-list.svg"},
            {SvgIcon.TextCode, "Text/Code.svg"},
            {SvgIcon.EditText, "Text/Edit-text.svg"},
            {SvgIcon.Filter, "Text/Filter.svg"},
            {SvgIcon.Font, "Text/Font.svg"},
            {SvgIcon.H1, "Text/H1.svg"},
            {SvgIcon.H2, "Text/H2.svg"},
            {SvgIcon.Itallic, "Text/Itallic.svg"},
            {SvgIcon.Menu, "Text/Menu.svg"},
            {SvgIcon.Paragraph, "Text/Paragraph.svg"},
            {SvgIcon.Quote1, "Text/Quote1.svg"},
            {SvgIcon.Quote2, "Text/Quote2.svg"},
            {SvgIcon.Redo, "Text/Redo.svg"},
            {SvgIcon.Strikethrough, "Text/Strikethrough.svg"},
            {SvgIcon.TextHeight, "Text/Text-height.svg"},
            {SvgIcon.TextWidth, "Text/Text-width.svg"},
            {SvgIcon.Text, "Text/Text.svg"},
            {SvgIcon.Underline, "Text/Underline.svg"},
            {SvgIcon.Undo, "Text/Undo.svg"},
            {SvgIcon.AngleGrinder, "Tools/Angle Grinder.svg"},
            {SvgIcon.Axe, "Tools/Axe.svg"},
            {SvgIcon.Brush, "Tools/Brush.svg"},
            {SvgIcon.Compass, "Tools/Compass.svg"},
            {SvgIcon.Hummer, "Tools/Hummer.svg"},
            {SvgIcon.Hummer2, "Tools/Hummer2.svg"},
            {SvgIcon.Pantone, "Tools/Pantone.svg"},
            {SvgIcon.RoadCone, "Tools/Road-Cone.svg"},
            {SvgIcon.Roller, "Tools/Roller.svg"},
            {SvgIcon.Roulette, "Tools/Roulette.svg"},
            {SvgIcon.Screwdriver, "Tools/Screwdriver.svg"},
            {SvgIcon.Shovel, "Tools/Shovel.svg"},
            {SvgIcon.Spatula, "Tools/Spatula.svg"},
            {SvgIcon.SwissKnife, "Tools/Swiss-knife.svg"},
            {SvgIcon.Tools, "Tools/Tools.svg"},
            {SvgIcon.Celcium, "Weather/Celcium.svg"},
            {SvgIcon.CloudFog, "Weather/Cloud-fog.svg"},
            {SvgIcon.CloudSun, "Weather/Cloud-sun.svg"},
            {SvgIcon.CloudWind, "Weather/Cloud-wind.svg"},
            {SvgIcon.Cloud1, "Weather/Cloud1.svg"},
            {SvgIcon.Cloud2, "Weather/Cloud2.svg"},
            {SvgIcon.CloudyNight, "Weather/Cloudy-night.svg"},
            {SvgIcon.Cloudy, "Weather/Cloudy.svg"},
            {SvgIcon.DayRain, "Weather/Day-rain.svg"},
            {SvgIcon.Fahrenheit, "Weather/Fahrenheit.svg"},
            {SvgIcon.Fog, "Weather/Fog.svg"},
            {SvgIcon.Moon, "Weather/Moon.svg"},
            {SvgIcon.NightFog, "Weather/Night-fog.svg"},
            {SvgIcon.NightRain, "Weather/Night-rain.svg"},
            {SvgIcon.Rain1, "Weather/Rain1.svg"},
            {SvgIcon.Rain2, "Weather/Rain2.svg"},
            {SvgIcon.Rain5, "Weather/Rain5.svg"},
            {SvgIcon.Rainbow, "Weather/Rainbow.svg"},
            {SvgIcon.Snow, "Weather/Snow.svg"},
            {SvgIcon.Snow1, "Weather/Snow1.svg"},
            {SvgIcon.Snow2, "Weather/Snow2.svg"},
            {SvgIcon.Snow3, "Weather/Snow3.svg"},
            {SvgIcon.Storm, "Weather/Storm.svg"},
            {SvgIcon.SunFog, "Weather/Sun-fog.svg"},
            {SvgIcon.Sun, "Weather/Sun.svg"},
            {SvgIcon.Suset1, "Weather/Suset1.svg"},
            {SvgIcon.Suset2, "Weather/Suset2.svg"},
            {SvgIcon.TemperatureEmpty, "Weather/Temperature-empty.svg"},
            {SvgIcon.TemperatureFull, "Weather/Temperature-full.svg"},
            {SvgIcon.TemperatureHalf, "Weather/Temperature-half.svg"},
            {SvgIcon.ThunderNight, "Weather/Thunder-night.svg"},
            {SvgIcon.ThunderDay, "Weather/Thunder.svg"},
            {SvgIcon.Umbrella, "Weather/Umbrella.svg"},
            {SvgIcon.Wind, "Weather/Wind.svg"},
        };

        internal static ConcurrentDictionary<SvgIcon, string> Cached = new ConcurrentDictionary<SvgIcon, string>();

        internal static string Get(SvgIcon Name)
        {
            if (Name != SvgIcon.None)
            {
                try
                {
                    if (Svg.Cached.TryGetValue(Name, out string iconCache))
                    {
                        return iconCache;
                    }
                    else
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "icons", "svg", Svg.All[Name]);

                        var svg = File.ReadAllText(path);

                        var match = new Regex("<svg(S*?)[^>]*>[.\\s\\S]*?</svg>", RegexOptions.IgnoreCase).Match(svg);

                        if (match.Success)
                        {
                            var icon = new Regex("<svg(S*?)[^>]*>").Replace(match.Value, "").Replace("</svg>", "").Replace("\n", "");

                            Svg.Cached.TryAdd(Name, icon);

                            return icon;
                        }
                    }
                }
                finally
                {

                }
            }

            return "";
        }
    }
}
