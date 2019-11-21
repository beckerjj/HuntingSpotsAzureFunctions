(function(){
    const $tbody = $("#theTable").find('tbody');

    api.getAll().then((list) => {
        list.sort((a,b) => {
            var nameA = a.name.toUpperCase(); // ignore upper and lowercase
            var nameB = b.name.toUpperCase(); // ignore upper and lowercase
            
            if (nameA < nameB) {
                return -1;
            }
            if (nameA > nameB) {
                return 1;
            }

            // names must be equal
            return 0;
        });
        let trHtml = list.map(renderRow).join('');

        $tbody.html(trHtml);
    });

    const renderRow = (data) => {
        return `
<tr>
    <td>${escapeHtml(data.name)}</td>
    <td>${data.longitude}</td>
    <td>${data.latitude}</td>
    <td>
        <a href="edit.html?rowKey=${data.rowKey}">Edit</a>
        <a href="delete.html?rowKey=${data.rowKey}">Delete</a>
    </td>
</tr>`;
    };
})()