﻿/*
 * CKFinder
 * ========
 * http://ckfinder.com
 * Copyright (C) 2007-2012, CKSource - Frederico Knabben. All rights reserved.
 *
 * The software, this file, and its contents are subject to the CKFinder
 * License. Please read the license.txt file before using, installing, copying,
 * modifying, or distributing this file or part of its contents. The contents of
 * this file is part of the Source Code of CKFinder.
 *
 */

/**
 * @fileOverview Defines the {@link CKFinder.lang} object for the Norwegian
 *		Bokmål language.
 */

/**
 * Contains the dictionary of language entries.
 * @namespace
 */
CKFinder.lang['nb'] =
    {
        appTitle: 'CKFinder',

        // Common messages and labels.
        common:
        {
            // Put the voice-only part of the label in the span.
            unavailable: '%1<span class="cke_accessibility">, utilgjenglig</span>',
            confirmCancel: 'Noen av valgene har blitt endret. Er du sikker på at du vil lukke dialogen?',
            ok: 'OK',
            cancel: 'Avbryt',
            confirmationTitle: 'Bekreftelse',
            messageTitle: 'Informasjon',
            inputTitle: 'Spørsmål',
            undo: 'Angre',
            redo: 'Gjør om',
            skip: 'Hopp over',
            skipAll: 'Hopp over alle',
            makeDecision: 'Hvilken handling skal utføres?',
            rememberDecision: 'Husk mitt valg'
        },

        // Language direction, 'ltr' or 'rtl'.
        dir: 'ltr',
        HelpLang: 'en',
        LangCode: 'nb',

        // Date Format
        //		d    : Day
        //		dd   : Day (padding zero)
        //		m    : Month
        //		mm   : Month (padding zero)
        //		yy   : Year (two digits)
        //		yyyy : Year (four digits)
        //		h    : Hour (12 hour clock)
        //		hh   : Hour (12 hour clock, padding zero)
        //		H    : Hour (24 hour clock)
        //		HH   : Hour (24 hour clock, padding zero)
        //		M    : Minute
        //		MM   : Minute (padding zero)
        //		a    : Firt char of AM/PM
        //		aa   : AM/PM
        DateTime: 'dd/mm/yyyy HH:MM',
        DateAmPm: ['AM', 'PM'],

        // Folders
        FoldersTitle: 'Mapper',
        FolderLoading: 'Laster...',
        FolderNew: 'Skriv inn det nye mappenavnet: ',
        FolderRename: 'Skriv inn det nye mappenavnet: ',
        FolderDelete: 'Er du sikker på at du vil slette mappen "%1"?',
        FolderRenaming: ' (Endrer mappenavn...)',
        FolderDeleting: ' (Sletter...)',

        // Files
        FileRename: 'Skriv inn det nye filnavnet: ',
        FileRenameExt: 'Er du sikker på at du vil endre filtypen? Filen kan bli ubrukelig.',
        FileRenaming: 'Endrer filnavn...',
        FileDelete: 'Er du sikker på at du vil slette denne filen "%1"?',
        FilesLoading: 'Laster...',
        FilesEmpty: 'Denne katalogen er tom.',
        FilesMoved: 'Filen %1 flyttet til %2:%3.',
        FilesCopied: 'Filen %1 kopiert til %2:%3.',

        // Basket
        BasketFolder: 'Kurv',
        BasketClear: 'Tøm kurv',
        BasketRemove: 'Fjern fra kurv',
        BasketOpenFolder: 'Åpne foreldremappen',
        BasketTruncateConfirm: 'Vil du virkelig fjerne alle filer fra kurven?',
        BasketRemoveConfirm: 'Vil du virkelig fjerne filen "%1" fra kurven?',
        BasketEmpty: 'Ingen filer i kurven, dra og slipp noen.',
        BasketCopyFilesHere: 'Kopier filer fra kurven',
        BasketMoveFilesHere: 'Flytt filer fra kurven',

        BasketPasteErrorOther: 'Fil %s feil: %e',
        BasketPasteMoveSuccess: 'Følgende filer ble flyttet: %s',
        BasketPasteCopySuccess: 'Følgende filer ble kopiert: %s',

        // Toolbar Buttons (some used elsewhere)
        Upload: 'Last opp',
        UploadTip: 'Last opp en ny fil',
        Refresh: 'Oppdater',
        Settings: 'Innstillinger',
        Help: 'Hjelp',
        HelpTip: 'Hjelp finnes kun på engelsk',

        // Context Menus
        Select: 'Velg',
        SelectThumbnail: 'Velg miniatyr',
        View: 'Vis fullversjon',
        Download: 'Last ned',

        NewSubFolder: 'Ny undermappe',
        Rename: 'Endre navn',
        Delete: 'Slett',

        CopyDragDrop: 'Kopier filen hit',
        MoveDragDrop: 'Flytt filen hit',

        // Dialogs
        RenameDlgTitle: 'Gi nytt navn',
        NewNameDlgTitle: 'Nytt navn',
        FileExistsDlgTitle: 'Filen finnes allerede',
        SysErrorDlgTitle: 'Systemfeil',

        FileOverwrite: 'Overskriv',
        FileAutorename: 'Gi nytt navn automatisk',

        // Generic
        OkBtn: 'OK',
        CancelBtn: 'Avbryt',
        CloseBtn: 'Lukk',

        // Upload Panel
        UploadTitle: 'Last opp ny fil',
        UploadSelectLbl: 'Velg filen du vil laste opp',
        UploadProgressLbl: '(Laster opp filen, vennligst vent...)',
        UploadBtn: 'Last opp valgt fil',
        UploadBtnCancel: 'Avbryt',

        UploadNoFileMsg: 'Du må velge en fil fra din datamaskin',
        UploadNoFolder: 'Vennligst velg en mappe før du laster opp.',
        UploadNoPerms: 'Filopplastning er ikke tillatt.',
        UploadUnknError: 'Feil ved sending av fil.',
        UploadExtIncorrect: 'Filtypen er ikke tillatt i denne mappen.',

        // Flash Uploads
        UploadLabel: 'Filer for opplastning',
        UploadTotalFiles: 'Totalt antall filer:',
        UploadTotalSize: 'Total størrelse:',
        UploadSend: 'Last opp',
        UploadAddFiles: 'Legg til filer',
        UploadClearFiles: 'Tøm filer',
        UploadCancel: 'Avbryt opplastning',
        UploadRemove: 'Fjern',
        UploadRemoveTip: 'Fjern !f',
        UploadUploaded: 'Lastet opp !n%',
        UploadProcessing: 'Behandler...',

        // Settings Panel
        SetTitle: 'Innstillinger',
        SetView: 'Filvisning:',
        SetViewThumb: 'Miniatyrbilder',
        SetViewList: 'Liste',
        SetDisplay: 'Vis:',
        SetDisplayName: 'Filnavn',
        SetDisplayDate: 'Dato',
        SetDisplaySize: 'Filstørrelse',
        SetSort: 'Sorter etter:',
        SetSortName: 'Filnavn',
        SetSortDate: 'Dato',
        SetSortSize: 'Størrelse',
        SetSortExtension: 'by Extension', // MISSING

        // Status Bar
        FilesCountEmpty: '<Tom Mappe>',
        FilesCountOne: '1 fil',
        FilesCountMany: '%1 filer',

        // Size and Speed
        Kb: '%1 kB',
        KbPerSecond: '%1 kB/s',

        // Connector Error Messages.
        ErrorUnknown: 'Det var ikke mulig å utføre forespørselen. (Feil %1)',
        Errors:
        {
            10: 'Ugyldig kommando.',
            11: 'Ressurstypen ble ikke spesifisert i forepørselen.',
            12: 'Ugyldig ressurstype.',
            102: 'Ugyldig fil- eller mappenavn.',
            103: 'Kunne ikke utføre forespørselen pga manglende autorisasjon.',
            104: 'Kunne ikke utføre forespørselen pga manglende tilgang til filsystemet.',
            105: 'Ugyldig filtype.',
            109: 'Ugyldig forespørsel.',
            110: 'Ukjent feil.',
            115: 'Det finnes allerede en fil eller mappe med dette navnet.',
            116: 'Kunne ikke finne mappen. Oppdater vinduet og prøv igjen.',
            117: 'Kunne ikke finne filen. Oppdater vinduet og prøv igjen.',
            118: 'Kilde- og mål-bane er like.',
            201: 'Det fantes allerede en fil med dette navnet. Den opplastede filens navn har blitt endret til "%1".',
            202: 'Ugyldig fil.',
            203: 'Ugyldig fil. Filen er for stor.',
            204: 'Den opplastede filen er korrupt.',
            205: 'Det finnes ingen midlertidig mappe for filopplastinger.',
            206: 'Opplastingen ble avbrutt av sikkerhetshensyn. Filen inneholder HTML-aktig data.',
            207: 'Den opplastede filens navn har blitt endret til "%1".',
            300: 'Klarte ikke å flytte fil(er).',
            301: 'Klarte ikke å kopiere fil(er).',
            500: 'Filvelgeren ikke tilgjengelig av sikkerhetshensyn. Kontakt systemansvarlig og be han sjekke CKFinder\'s konfigurasjonsfil.',
            501: 'Funksjon for minityrbilder er skrudd av.'
        },

        // Other Error Messages.
        ErrorMsg:
        {
            FileEmpty: 'Filnavnet kan ikke være tomt.',
            FileExists: 'Filen %s finnes alt.',
            FolderEmpty: 'Mappenavnet kan ikke være tomt.',

            FileInvChar: 'Filnavnet kan ikke inneholde følgende tegn: \n\\ / : * ? " < > |',
            FolderInvChar: 'Mappenavnet kan ikke inneholde følgende tegn: \n\\ / : * ? " < > |',

            PopupBlockView: 'Du må skru av popup-blockeren for å se bildet i nytt vindu.',
            XmlError: 'Det var ikke mulig å laste XML-dataene i svaret fra serveren.',
            XmlEmpty: 'Det var ikke mulig å laste XML-dataene fra serverne, svaret var tomt.',
            XmlRawResponse: 'Rått datasvar fra serveren: %s'
        },

        // Imageresize plugin
        Imageresize:
        {
            dialogTitle: 'Endre størrelse %s',
            sizeTooBig: 'Cannot set image height or width to a value bigger than the original size (%size).', // MISSING
            resizeSuccess: 'Endring av bildestørrelse var vellykket.',
            thumbnailNew: 'Lag ett nytt miniatyrbilde',
            thumbnailSmall: 'Liten (%s)',
            thumbnailMedium: 'Medium (%s)',
            thumbnailLarge: 'Stor (%s)',
            newSize: 'Sett en ny størrelse',
            width: 'Bredde',
            height: 'Høyde',
            invalidHeight: 'Ugyldig høyde.',
            invalidWidth: 'Ugyldig bredde.',
            invalidName: 'Ugyldig filnavn.',
            newImage: 'Lag ett nytt bilde',
            noExtensionChange: 'Filendelsen kan ikke endres.',
            imageSmall: 'Kildebildet er for lite.',
            contextMenuName: 'Endre størrelse',
            lockRatio: 'Lås forhold',
            resetSize: 'Tilbakestill størrelse'
        },

        // Fileeditor plugin
        Fileeditor:
        {
            save: 'Lagre',
            fileOpenError: 'Klarte ikke å åpne filen.',
            fileSaveSuccess: 'Fillagring var vellykket.',
            contextMenuName: 'Rediger',
            loadingFile: 'Laster fil, vennligst vent...'
        },

        Maximize:
        {
            maximize: 'Maksimer',
            minimize: 'Minimer'
        }
    };