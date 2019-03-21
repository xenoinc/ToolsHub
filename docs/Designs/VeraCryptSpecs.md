# Introduction
VeraCrypt add-in allows you to mount your encrypted virtual drives to keep prying eyes out of your projects. Or in my case, stop CrapAfee from encrypting your virtual drives and disabling you from transferring them to your new laptop (_jerks!_).

## Requested Features

| Feature | Status | Description |
|---------|--------|-------------|
| Mount VC container | TBD | Quickly mount VC and prompt you for your password |
| Password store | TBD | Mount drives on-demand and auto fill-in your password |
| Auto-mount | TBD | Automatically mount your selected drive(s) when ToolsHub starts |
| Fingerprints | TBD | Mount and authenticate via your device's fingerprint scanner |
| YubiKey (2-auth) | TBD | Bypass your manual password and use your YubiKey, or 2-auth |


# Command Line Usage
As of 2019-03-07, the following command line parameters are accepted. Reference material, [VeraCrypt docs](https://www.veracrypt.fr/en/Command%20Line%20Usage.html).


Note that this section applies to the Windows version of VeraCrypt. For information on command line usage applying to the Linux and Mac OS X versions, please run: veracrypt –h

## Common Commands
| Command | Description |
|:--------|:------------|
| /help or /? | Display command line help. |
| /truecrypt or /tc | Activate TrueCrypt compatibility mode which enables mounting volumes created with TrueCrypt 6.x and 7.x series.
| /hash | It must be followed by a parameter indicating the PRF hash algorithm to use when mounting the volume. Possible values for /hash parameter are: sha256, sha-256, sha512, sha-512, whirlpool, ripemd160 and ripemd-160. When /hash is omitted, VeraCrypt will try all possible PRF algorithms thus lengthening the mount operation time. |
| /volume or /v | <p>It must be followed by a parameter indicating the file and path name of a VeraCrypt volume to mount (do not use when dismounting) or the Volume ID of the disk/partition to mount.</p><p>The syntax of the volume ID is **ID:XXXXXX...XX** where the XX part is a 64 hexadecimal characters string that represent the 32-Bytes ID of the desired volume to mount.</p><p>To mount a partition/device-hosted volume, use, for example, ```/v \Device\Harddisk1\Partition3``` (to determine the path to a partition/device, run VeraCrypt and click Select Device). You can also mount a partition or dynamic volume using its volume name (for example, ``/v \\?\Volume{5cceb196-48bf-46ab-ad00-70965512253a}\)``. To determine the volume name use e.g. mountvol.exe. Also note that device paths are case-sensitive.</p><bp>You can also specify the Volume ID of the partition/device-hosted volume to mount, for example: ``/v ID:53B9A8D59CC84264004DA8728FC8F3E2EE6C130145ABD3835695C29FD601EDCA``. The Volume ID value can be retrieved using the volume properties dialog.</p> |
| /letter or /l | It must be followed by a parameter indicating the driver letter to mount the volume as. When ``/l`` is omitted and when /a is used, the first free drive letter is used.
| /explore or /e | Open an Explorer window after a volume has been mounted. |
| /beep or /b | Beep after a volume has been successfully mounted or dismounted. |
| /auto or /a | If no parameter is specified, automatically mount the volume. If devices is specified as the parameter (e.g., ``/a devices``), auto-mount all currently accessible device/partition-hosted VeraCrypt volumes. If favorites is specified as the parameter, auto-mount favorite volumes. Note that /auto is implicit if ``/quit`` and ``/volume`` are specified. If you need to prevent the application window from appearing, use ``/quit.`` |
| /dismount or /d | Dismount volume specified by drive letter (e.g., ``/d x``). When no drive letter is specified, dismounts all currently mounted VeraCrypt volumes. |
| /force or /f | Forces dismount (if the volume to be dismounted contains files being used by the system or an application) and forces mounting in shared mode (i.e., without exclusive access). |
| /keyfile or /k | It must be followed by a parameter specifying a keyfile or a keyfile search path. For multiple keyfiles, specify e.g.: ``/k c:\keyfile1.dat /k d:\KeyfileFolder /k c:\kf2`` To specify a keyfile stored on a security token or smart card, use the following syntax: ``token://slot/SLOT_NUMBER/file/FILE_NAME`` |
| /tryemptypass   | <p>ONLY when default keyfile configured or when a keyfile is specified in the command line.</p><p>If it is followed by ``y`` or ``yes`` or if no parameter is specified: try to mount using an empty password and the keyfile before displaying password prompt.</p><p>If it is followed by ``n`` or ``no``: don't try to mount using an empty password and the keyfile, and display password prompt right away.</p> |
| /nowaitdlg | <p>If it is followed by ``y`` or ``yes`` or if no parameter is specified: don’t display the waiting dialog while performing operations like mounting volumes.</p><p>If it is followed by ``n`` or ``no``: force the display waiting dialog is displayed while performing operations.</p> |
| /secureDesktop | <p>If it is followed by y or yes or if no parameter is specified: display password dialog in a dedicated secure desktop to protect against certain types of attacks.</p><p>If it is followed by n or no: the password dialog is displayed in the normal desktop.</p> |
| /tokenlib | <p>It must be followed by a parameter indicating the PKCS #11 library to use for security tokens and smart cards. (e.g.: ``/tokenlib c:\pkcs11lib.dll``)</p> |
| /tokenpin | <p>It must be followed by a parameter indicating the PIN to use in order to authenticate to the security token or smart card (e.g.: ``/tokenpin 0000``). **Warning:** This method of entering a smart card PIN may be insecure, for example, when an unencrypted command prompt history log is being saved to unencrypted disk.</p> |
| /cache or /c | <p>If it is followed by ``y`` or ``yes`` or if no parameter is specified: enable password cache;</p><p>If it is followed by ``n`` or ``no``: disable password cache (e.g., ``/c n``).</p><p>If it is followed by ``f`` or ``favorites``: temporary cache password when mounting multiple favorites  (e.g., ``/c f``).</p><p>**Note** that turning the password cache off will not clear it (use ``/w`` to clear the password cache).</p> |
| /history or /h | <p>If it is followed by y or no parameter: enables saving history of mounted volumes; if it is followed by n: disables saving history of mounted volumes (e.g., ``/h n``).</p> |
| /wipecache or /w | <p>Wipes any passwords cached in the driver memory.</p> |
| /password or /p | <p>It must be followed by a parameter indicating the volume password. If the password contains spaces, it must be enclosed in quotation marks (e.g., ``/p ”My Password”``). Use ``/p ””`` to specify an empty password.</p><p>**Warning:** This method of entering a volume password may be insecure, for example, when an unencrypted command prompt history log is being saved to unencrypted disk.</p> |
| /pim | It must be followed by a positive integer indicating the PIM (Personal Iterations Multiplier) to use for the volume. |
| /quit or /q | Automatically perform requested actions and exit (main VeraCrypt window will not be displayed). If preferences is specified as the parameter (e.g., ``/q preferences``), then program settings are loaded/saved and they override settings specified on the command line. ``/q`` background launches the VeraCrypt Background Task (tray icon) unless it is disabled in the Preferences. |
| /silent or /s | <p>If ``/q`` is specified, suppresses interaction with the user (prompts, error messages, warnings, etc.). If ``/q`` is not specified, this option has no effect.</p> |
| /mountoption or /m | <p>It must be followed by a parameter which can have one of the values indicated below.</p><p>``ro`` or ``readonly``: Mount volume as read-only.</p><p>``rm`` or ``removable``: Mount volume as removable medium (see section Volume Mounted as Removable Medium).</p><p>``ts`` or ``timestamp``: Do not preserve container modification timestamp.</p><p>``sm`` or ``system``: Without pre-boot authentication, mount a partition that is within the key scope of system encryption (for example, a partition located on the encrypted system drive of another operating system that is not running). Useful e.g. for backup or repair operations. Note: If you supply a password as a parameter of ``/p``, make sure that the password has been typed using the standard US keyboard layout (in contrast, the GUI ensures this automatically). This is required due to the fact that the password needs to be typed in the pre-boot environment (before Windows starts) where non-US Windows keyboard layouts are not available.</p><p>``bk`` or ``headerbak``: Mount volume using embedded backup header. Note: All volumes created by VeraCrypt contain an embedded backup header (located at the end of the volume).</p><p>``recovery``: Do not verify any checksums stored in the volume header. This option should be used only when the volume header is damaged and the volume cannot be mounted even with the mount option headerbak. Example: ``/m ro``</p><p>``label=LabelValue``: Use the given string value LabelValue as a label of the mounted volume in Windows Explorer. The maximum length for **LabelValue** is *32 characters* for NTFS volumes and *11 characters* for FAT volumes. For example, ``/m label=MyDrive`` will set the label of the drive in Explorer to MyDrive.</p><p>``noattach``: Only create virtual device without actually attaching the mounted volume to the selected drive letter.</p><p>Please note that this switch may be present several times in the command line in order to specify multiple mount options (e.g.: ``/m rm /m ts``) |

## VeraCrypt Format.exe

VeraCrypt Format.exe (VeraCrypt Volume Creation Wizard):

***TODO: FORMAT THE FOLLOWING TABLE***

| Command | Description |
|:----|---|:------------|
| /create | Create a container based volume in command line mode. It must be followed by the file name of the container to be created. |
| /size | <p>(Only with /create)</p><p>It must be followed by a parameter indicating the size of the container file that will be created. This parameter is a number indicating the size in Bytes. It can have a suffixe 'K', 'M', 'G' or 'T' to indicate that the value is in Kilobytes, Megabytes, Gigabytes or Terabytes respectively. For example:<ul><li>``/size 5000000``: the container size will be 5000000 bytes</li><li>``/size 25K``: the container size will be 25 KiloBytes.</li><li>``/size 100M``: the container size will be 100 MegaBytes.</li><li>``/size 2G``: the container size will be 2 GigaBytes.</li><li>``/size 1T``: the container size will be 1 TeraBytes.</li></ul> |
| /password	| <p>(Only with /create)</p><p>It must be followed by a parameter indicating the password of the container that will be created.</p> |
| /hash | <p>(Only with /create)</p><p>It must be followed by a parameter indicating the PRF hash algorithm to use when creating the volume. It has the same syntax as VeraCrypt.exe.</p> |
/encryption	(Only with /create)
It must be followed by a parameter indicating the encryption algorithm to use. The default is AES if this switch is not specified. The parameter can have the following values (case insensitive):<ul>
<li>AES</li><li>Serpent</li><li>Twofish</li><li>Camellia</li><li>Kuznyechik</li><li>AES(Twofish)</li><li>AES(Twofish(Serpent))</li><li>Serpent(AES)</li><li>Serpent(Twofish(AES))</li><li>Twofish(Serpent)</li><li>Camellia(Kuznyechik)</li><li>Kuznyechik(Twofish)</li><li>Camellia(Serpent)</li><li>Kuznyechik(AES)</li><li>Kuznyechik(Serpent(Camellia))</li><li></ul>
/filesystem	(Only with /create)
It must be followed by a parameter indicating the file system to use for the volume. The parameter can have the following values:
None: don't use any filesystem
FAT: format using FAT/FAT32
NTFS: format using NTFS. Please note that in this case a UAC prompt will be displayed unless the process is run with full administrative privileges.
ExFAT: format using ExFAT. This switch is available starting from Windows Vista SP1
ReFS: format using ReFS. This switch is available starting from Windows 10
/dynamic	(Only with /create)
It has no parameters and it indicates that the volume will be created as a dynamic volume.
/force	(Only with /create)
It has no parameters and it indicates that overwrite will be forced without requiring user confirmation.
/silent	(Only with /create)
It has no parameters and it indicates that no message box or dialog will be displayed to the user. If there is any error, the operation will fail silently.
/noisocheck or /n	Do not verify that VeraCrypt Rescue Disks are correctly burned. WARNING: Never attempt to use this option to facilitate the reuse of a previously created VeraCrypt Rescue Disk. Note that every time you encrypt a system partition/drive, you must create a new VeraCrypt Rescue Disk even if you use the same password. A previously created VeraCrypt Rescue Disk cannot be reused as it was created for a different master key.
```

## Syntax

### VeraCrypt
```
VeraCrypt.exe [/tc] [/hash {sha256|sha-256|sha512|sha-512|whirlpool |ripemd160|ripemd-160}][/a [devices|favorites]] [/b] [/c [y|n|f]] [/d [drive letter]] [/e] [/f] [/h [y|n]] [/k keyfile or search path] [tryemptypass [y|n]] [/l drive letter] [/m {bk|rm|recovery|ro|sm|ts|noattach}] [/p password] [/pim pimvalue] [/q [background|preferences]] [/s] [/tokenlib path] [/v volume] [/w]
```

### VeraCrypt Format
```
"VeraCrypt Format.exe" [/n] [/create] [/size number[{K|M|G|T}]] [/p password]  [/encryption {AES | Serpent | Twofish | Camellia | Kuznyechik | AES(Twofish) | AES(Twofish(Serpent)) | Serpent(AES) | Serpent(Twofish(AES)) | Twofish(Serpent) | Camellia(Kuznyechik) | Kuznyechik(Twofish) | Camellia(Serpent) | Kuznyechik(AES) | Kuznyechik(Serpent(Camellia))}] [/hash {sha256|sha-256|sha512|sha-512|whirlpool|ripemd160|ripemd-160}] [/filesystem {None|FAT|NTFS|ExFAT|ReFS}] [/dynamic] [/force] [/silent]
```

Note that the order in which options are specified does not matter.

## Examples

### Mount with prompt
Mount the volume ``d:\myvolume`` as the first free drive letter, using the password prompt (the main program window will not be displayed):

```
veracrypt /q /v d:\myvolume
```

### Dismount
Dismount a volume mounted as the drive letter ``X`` (the main program window will not be displayed):

```
veracrypt /q /d x
```

### Mount with password
Mount a volume called ``myvolume.tc`` using the password ``MyPassword``, as the drive letter ``X``. VeraCrypt will open an explorer window and beep; mounting will be automatic:

```
veracrypt /v myvolume.tc /l x /a /p MyPassword /e /b
```

### Create Volume
Create a ``10 MB`` file container using the password ``test`` and formatted using ``FAT``:

```
"C:\Program Files\VeraCrypt\VeraCrypt Format.exe" /create c:\Data\test.hc /password test /hash sha512 /encryption serpent /filesystem FAT /size 10M /force
```
