

//多语种退出平台消息
function getExitMsgByLangCode() {

    var varLangCode = getcookie("LangCode");
    var varMsg;

    if (varLangCode == "zh-CN") {
        varMsg = '确定要退出管理平台吗?';
    }
    else if (varLangCode == "zh-tw") {
        varMsg = '確定要退出管理平台嗎?';
    }
    else if (varLangCode == "en") {
        varMsg = 'Are you sure you want to quit the management platform?';
    }
    else if (varLangCode == "de") {
        varMsg = 'Möchten Sie die Verwaltungsplattform wirklich beenden?';
    }
    else if (varLangCode == "es") {
        varMsg = '¿Estás seguro de que quieres abandonar la plataforma de gestión?';
    }
    else if (varLangCode == "fr") {
        varMsg = 'Êtes-vous sûr de vouloir quitter la plate-forme de gestion?';
    }
    else if (varLangCode == "it") {
        varMsg = 'Sei sicuro di voler lasciare la piattaforma di gestione?';
    }
    else if (varLangCode == "ja") {
        varMsg = '管理プラットフォームを終了してもよろしいですか？';
    }
    else if (varLangCode == "ko") {
        varMsg = '관리 플랫폼을 종료 하시겠습니까?';
    }
    else if (varLangCode == "pt") {
        varMsg = 'Tem certeza de que deseja sair da plataforma de gerenciamento?';
    }
    else if (varLangCode == "ru") {
        varMsg = 'Вы действительно хотите выйти из платформы управления?';
    }
    else {
        varMsg = 'Are you sure you want to quit the management platform?';
    }

    return varMsg;

}

//多语种删除消息

function getDeleteMsgByLangCode() {

    var varLangCode = getcookie("LangCode");
    var varMsg;

    if (varLangCode == "zh-CN") {
        varMsg = '确定要删除吗?';
    }
    else if (varLangCode == "zh-tw") {
        varMsg = '確定要刪除嗎？';
    }
    else if (varLangCode == "en") {
        varMsg = 'Are you sure you want to delete?';
    }
    else if (varLangCode == "de") {
        varMsg = 'Sind Sie sicher, dass Sie löschen möchten?';
    }
    else if (varLangCode == "es") {
        varMsg = '¿Estás seguro de que quieres eliminarlo?';
    }
    else if (varLangCode == "fr") {
        varMsg = 'Êtes-vous sûr de vouloir supprimer ?';
    }
    else if (varLangCode == "it") {
        varMsg = 'Sei sicuro di voler eliminare?';
    }
    else if (varLangCode == "ja") {
        varMsg = '削除してもよろしいですか?';
    }
    else if (varLangCode == "ko") {
        varMsg = '삭제하시겠습니까?';
    }
    else if (varLangCode == "pt") {
        varMsg = 'Tem certeza de que deseja excluir?';
    }
    else if (varLangCode == "ru") {
        varMsg = 'Вы уверены, что хотите удалить?';
    }
    else {
        varMsg = 'Are you sure you want to delete?';
    }

    return varMsg;
}

//多语种项目计划复制消息
function getCopyProjectPlanMsgByLangCode() {

    var varLangCode = getcookie("LangCode");
    var varMsg;

    if (varLangCode == "zh-CN") {
        varMsg = '复制操作会完全覆盖原来的计划数据，您确定要复制吗?';
    }
    else if (varLangCode == "zh-tw") {
        varMsg = '複製操作會完全覆蓋原來的計劃數據，您確定要複製嗎？';
    }
    else if (varLangCode == "en") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "de") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "es") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "fr") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "it") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "ja") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "ko") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "pt") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else if (varLangCode == "ru") {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }
    else {
        varMsg = 'Copy operation will delete all old plan data,Are you sure you want to copy it?';
    }

    return varMsg;
}

//多语种项目计划启动消息
function getStartupProjectPlanMsgByLangCode() {

    var varLangCode = getcookie("LangCode");
    var varMsg;

    if (varLangCode == "zh-CN") {
        varMsg = '启动后，计划内容不能更改，确定要启动吗?';
    }
    else if (varLangCode == "zh-tw") {
        varMsg = '啟動後，計劃內容不能更改，確定要啟動嗎？';
    }
    else if (varLangCode == "en") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "de") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "es") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "fr") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "it") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "ja") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "ko") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "pt") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else if (varLangCode == "ru") {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }
    else {
        varMsg = 'After launching, the content of the plan cannot be changed, are you sure you want to start?';
    }

    return varMsg;
}

//多语种项目计划拼接消息
function getJoinProjectPlanMsgByLangCode() {

    var varLangCode = getcookie("LangCode");
    var varMsg;

    if (varLangCode == "zh-CN") {
        varMsg = '拼接操作会影响原来的计划结构，您确定要拼接吗?';
    }
    else if (varLangCode == "zh-tw") {
        varMsg = '拼接操作會影響原來的計劃結構，您確定要拼接嗎';
    }
    else if (varLangCode == "en") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "de") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "es") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "fr") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "it") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "ja") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "ko") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "pt") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else if (varLangCode == "ru") {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }
    else {
        varMsg = 'Joint operation will impact old plan data,Are you sure you want to joint it?';
    }

    return varMsg;

}
