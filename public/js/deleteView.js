(function(){
    const rowKey = getQueryVariable('rowKey');
    const $deleteForm = $("#deleteForm");

    api.getOne(rowKey).then((record) => {
        populateForm($deleteForm, record);
    });

    $deleteForm.on('submit', e => {
        let data = jsonSerializeForm($deleteForm);

        api.deleteOne(data).then(r => {
            window.location = '/';
        });

        return false;
    });
})()