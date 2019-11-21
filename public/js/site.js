const config = {
    urls: {
        get: '/api/get',
        getOne: '/api/getOne',
        create: '/api/create',
        edit: '/api/edit',
        delete: '/api/delete',
    }
};

const escapeHtml = function () {
    let regex = /[&<>"']/g,
        replacementMap = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&apos;'
        },
        replacer = function(part) {
            return replacementMap[part];
        };

    return function (str) {
        return str.replace(regex, replacer);
    };
}();

const getQueryVariable = function (variable) {
    var query = window.location.search.substring(1);
    var vars = query.split('&');
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split('=');
        if (decodeURIComponent(pair[0]) == variable) {
            return decodeURIComponent(pair[1]);
        }
    }
    console.log('Query variable %s not found', variable);
}

const populateForm = (form, data) => {
    $(form).find('input:not([type=submit])').each((i, element) => {
        let name = element.name;
        let value = data[name];

        $(element).val(value);
    });
    $(form).find('span').each((i, element) => {
        let name = element.attributes.name.value;
        let value = data[name];

        $(element).text(value);
    });
};

const jsonSerializeForm = (form) => {
    let serializedArray = $(form).serializeArray();
    let objEntries = serializedArray.map(({name,value}) => [name,value]);
    let obj = Object.fromEntries(objEntries);
    return obj;
};

const api = {
    myFetch: function (url, data, fetchOptions, errorMessage = 'An error occured!') {
        let body = undefined;
    
        if (data) {
            if (fetchOptions && fetchOptions.method === 'GET') {
                let params = new URLSearchParams();
                for (let [key, value] of Object.entries(data)) {
                    params.append(key, value);
                }
                url += params.toString();
            } else {
                body = JSON.stringify(data);
            }
        }
    
        fetchOptions = Object.assign({
                method: data ? 'POST' : 'GET',
                body,
                headers: {
                    'Content-Type': 'application/json',
                    'Proxy-Trace-Enabled': 'true'
                },
                //credentials: 'same-origin'
            },
            fetchOptions);
    
        let promise = window.fetch(url, fetchOptions)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
    
                if (response.status === 204) {
                    return null;
                }
    
                return response.json().catch(e => {
                    throw new Error('Failed parsing server response to json.');
                });
            })
            .catch(error => {
                //Todo: global handling the error, some kind of toast?
                alert(`${errorMessage} - ${error.message}`);
                throw error;
            });
    
        return promise;
    },
    
    getAll: function () {
        return api.myFetch(config.urls.get, null, null, 'Failed retrieving spots!');
    },
    
    getOne: function (rowKey) {
        return api.myFetch(config.urls.getOne + '?', { rowKey }, { method : 'GET' }, 'Failed retrieving spot!');
    },
    
    createOne: function (data) {
        return api.myFetch(config.urls.create, data, 'Failed creating spot!');
    },
    
    editOne: function (data) {
        return api.myFetch(config.urls.edit, data, 'Failed editing spot!');
    },
    
    deleteOne: function (data) {
        return api.myFetch(config.urls.delete, data, 'Failed deleting spot!');
    }
};