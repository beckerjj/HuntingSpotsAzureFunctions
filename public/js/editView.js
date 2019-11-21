(function(){
    const rowKey = getQueryVariable('rowKey');
    const $editForm = $("#editForm");

    api.getOne(rowKey).then((record) => {
        populateForm($editForm, record);
    });

    $editForm.on('submit', e => {
        let data = jsonSerializeForm($editForm);

        api.editOne(data).then(r => {
            window.location = '/';
        });

        return false;
    });
})()