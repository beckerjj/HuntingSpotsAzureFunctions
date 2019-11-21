(function(){
    const latitude = getQueryVariable('lat');
    const longitude = getQueryVariable('long');
    const $createForm = $("#createForm");

    if (latitude || longitude) {
        populateForm($createForm, { latitude, longitude });
    }

    $createForm.on('submit', e => {
        let data = jsonSerializeForm($createForm);

        api.createOne(data).then(r => {
            window.location = '/';
        });

        return false;
    });
})()