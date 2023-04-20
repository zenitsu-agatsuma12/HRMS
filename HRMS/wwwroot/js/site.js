
//Show the Position inside the Department
function FillPosition(listDepartmentCtrl, listPositionCtrl) {
    var listPosition = $("#" + listPositionCtrl);
    listPosition.empty();

    var selectedDepartment = listDepartmentCtrl.options[listDepartmentCtrl.selectedIndex].value;

    if (selectedDepartment != null && selectedDepartment != '') {
        $.getJSON("/Account/GetPositionByDepartment", { departmentId: selectedDepartment }, function (positions) {
            if (positions != null && positions.length > 0) {
                $.each(positions, function (index, position) {
                    listPosition.append($('<option/>', {
                        value: position.value,
                        text: position.text
                    }));
                });
            }
            else {
                listPosition.append($('<option/>', {
                    value: '',
                    text: 'No positions found'
                }));
            }
        });
    }
    else {
        listPosition.append($('<option/>', {
            value: '',
            text: 'Select a department first'
        }));
    }
}

//Benefits Format
const sssNumberInput = document.getElementById('SSSNumber');
sssNumberInput.addEventListener('input', function (e) {
    const input = e.target.value;
    const formattedInput = input.replace(/\D/g, '').replace(/(\d{2})(\d{7})(\d{1})/, '$1-$2-$3');
    e.target.value = formattedInput;
});
//PhilHealth
const philHealthIdInput = document.getElementById('PhilHealthId');
philHealthIdInput.addEventListener('input', function (e) {
    const input = e.target.value;
    const formattedInput = input.replace(/\D/g, '').replace(/(\d{2})(\d{9})(\d{1})/, '$1-$2-$3');
    e.target.value = formattedInput;
});
//Pag-Ibig
const pagIbigIdInput = document.getElementById('PagIbigId');
pagIbigIdInput.addEventListener('input', function (e) {
    const input = e.target.value;
    const formattedInput = input.replace(/\D/g, '').replace(/(\d{4})(\d{4})(\d{4})/, '$1-$2-$3');
    e.target.value = formattedInput;
});

