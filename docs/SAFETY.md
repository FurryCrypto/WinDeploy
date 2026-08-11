# ESD Installer safety model

## Invariants

- No destructive target is selected automatically.
- A drive letter is never the sole identity of a destructive target.
- The running Windows partition, boot/system partitions, EFI, MSR, recovery, OEM, offline, read-only, unallocated, and detected BitLocker targets are not selectable.
- The destination boot partition must belong to the same physical disk as the destination Windows partition.
- An existing EFI partition may receive Windows boot files but is never formatted.
- Formatting receives a partition object obtained only after stable disk identity and exact partition geometry checks.
- Source, image index, disk, target partition, boot partition, firmware mode, and partition scheme are revalidated after elevation.
- A nonzero DISM or BCDBoot exit code is a failure.
- Missing deployed registry hives, loader files, boot manager files, UEFI fallback files, or BIOS BCD stores are failures.
- Windows 11 TPM, Secure Boot capability, memory, storage, CPU, and UEFI policy checks are blocked by default. Advanced Mode can bypass them only after a separate explicit warning and records that choice in the immutable plan and installation log. Architecture, exact disk identity, partition geometry, current-system protection, and boot-layout checks cannot be bypassed.

## Intentional limitations

- The application does not create, resize, delete, or convert partitions.
- Unallocated space is visual context only.
- Split SWM media is not accepted in this release.
- Vista, XP, and unrecognized images are detected but routed to unavailable legacy engines.
- BitLocker targets must be fully decrypted before deployment.
- One-time UEFI firmware variables are not changed. Secondary-disk UEFI installs use and verify the standard fallback boot path.
- Full destructive end-to-end testing requires disposable hardware or a purpose-built virtual machine with nested firmware and expendable virtual disks. The included automated suite is deliberately read-only.

## Recovery expectations

Formatting and image application are not transactional and cannot be rolled back. If DISM, BCDBoot, power, storage, or hardware fails after formatting begins, the destination partition can be incomplete. ESD Installer preserves its detailed log and reports failure; it never converts an incomplete deployment into a success message.
