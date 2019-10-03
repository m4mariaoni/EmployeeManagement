function confirmDelete(uniqueId, confirmDelete)
{
    var deleteSpan = 'deleteSpan_' + uniqueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + uniqueId;

    if (confirmDelete) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    }
    else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();

    }
}