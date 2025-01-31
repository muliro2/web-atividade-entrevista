﻿let beneficiarios = [];
$(document).ready(function () {
    if (obj) {
        $('#formCadastro #Nome').val(obj.Nome);
        $('#formCadastro #CEP').val(obj.CEP);
        $('#formCadastro #Email').val(obj.Email);
        $('#formCadastro #Sobrenome').val(obj.Sobrenome);
        $('#formCadastro #Nacionalidade').val(obj.Nacionalidade);
        $('#formCadastro #Estado').val(obj.Estado);
        $('#formCadastro #Cidade').val(obj.Cidade);
        $('#formCadastro #Logradouro').val(obj.Logradouro);
        $('#formCadastro #Telefone').val(obj.Telefone);
        $('#formCadastro #CPF').val(obj.CPF);
    }

    updateList();

    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        const cpf = $(this).find("#CPF").val();

        if (!isValidCPF(cpf)) {
            ModalDialog("CPF inválido", "O campo CPF informado é inválido, informe um CPF válido.");
        } else {
            $.ajax({
                url: urlPost,
                method: "POST",
                data: {
                    "NOME": $(this).find("#Nome").val(),
                    "CEP": $(this).find("#CEP").val(),
                    "Email": $(this).find("#Email").val(),
                    "Sobrenome": $(this).find("#Sobrenome").val(),
                    "Nacionalidade": $(this).find("#Nacionalidade").val(),
                    "Estado": $(this).find("#Estado").val(),
                    "Cidade": $(this).find("#Cidade").val(),
                    "Logradouro": $(this).find("#Logradouro").val(),
                    "Telefone": $(this).find("#Telefone").val(),
                    "CPF": $(this).find("#CPF").val(),
                    "Beneficiarios": beneficiarios
                },
                error:
                    function (r) {
                        if (r.status == 400)
                            ModalDialog("Ocorreu um erro", r.responseJSON);
                        else if (r.status == 500)
                            ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                    },
                success:
                    function (r) {
                        ModalDialog("Sucesso!", r)
                        $("#formCadastro")[0].reset();
                        window.location.href = urlRetorno;
                    }
            });
        }
    })

    $("#formBeneficiario").on("submit", function (e) {
        e.preventDefault();

        const id = $("#BeneficiarioId").val();
        const cpf = $("#BeneficiarioCPF").val();
        const nome = $("#BeneficiarioNome").val();

        if (!isValidCPF(cpf)) {
            ModalDialog("CPF inválido", "O campo CPF informado é inválido, informe um CPF válido.");
        } else {
            if (!beneficiaryAlreadyExist(cpf)) {
                const beneficiario = { id, cpf, nome };
                beneficiarios.push(beneficiario);

                updateBeneficiaryList();

                $("#BeneficiarioId").val('');
                $("#BeneficiarioCPF").val('');
                $("#BeneficiarioNome").val('');
            } else {
                ModalDialog("CPF inválido", "Já existe um beneficiario com o CPF informado.");
            }
        }
    });

    window.editBeneficiario = function (index) {

        const beneficiario = beneficiarios[index];

        if (beneficiario) {
            $("#BeneficiarioId").val(beneficiario.id);
            $("#BeneficiarioCPF").val(beneficiario.cpf);
            $("#BeneficiarioNome").val(beneficiario.nome);

            beneficiarios.splice(index, 1);

            updateBeneficiaryList();
        } else {
            console.error("Índice inválido: " + index);
        }
    };

    window.delBeneficiario = function (index) {
        const beneficiario = beneficiarios[index];

        beneficiario.Action = 1;

        updateBeneficiaryList();
    };

    $('#CPF').mask('000.000.000-00');
    $('#BeneficiarioCPF').mask('000.000.000-00');
})

function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}

function isValidCPF(strCPF) {
    var Soma;
    var Resto;
    Soma = 0;
    strCPF = strCPF.replace(/\D/g, '');

    if (strCPF == "00000000000") return false;

    for (i = 1; i <= 9; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(9, 10))) return false;

    Soma = 0;
    for (i = 1; i <= 10; i++) Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i);
    Resto = (Soma * 10) % 11;

    if ((Resto == 10) || (Resto == 11)) Resto = 0;
    if (Resto != parseInt(strCPF.substring(10, 11))) return false;
    return true;
}


function updateList() {
    beneficiarios = [];

    $('#beneficiariosTable tbody tr').each(function () {
        var beneficiario = {
            Id: $(this).attr('id'),
            CPF: $(this).find('td:eq(0)').text(),
            Nome: $(this).find('td:eq(1)').text()
        };
        beneficiarios.push(beneficiario);
    });
}

function updateBeneficiaryList() {
    const tbody = $("#beneficiariosTable tbody");
    tbody.empty();

    beneficiarios.forEach((beneficiario, index) => {
        if (beneficiario.Action !== 1) {
            const row = `<tr id="${beneficiario.id}">
                            <td>${beneficiario.cpf}</td>
                            <td>${beneficiario.nome}</td>
                            <td>
                                <button type="button" class="btn btn-sm btn-primary" onclick="editBeneficiario(${index})" id="editarBeneficiario" value="${beneficiario.id}">Alterar</button>
                                <button type="button" class="btn btn-sm btn-primary" onclick="delBeneficiario(${index})" id="excluirBeneficiario" value="${beneficiario.id}">Excluir</button>
                            </td>
                        </tr>`;
            tbody.append(row);
        }
    });
}

function beneficiaryAlreadyExist(cpf) {
    return beneficiarios.some(beneficiario => beneficiario.cpf === cpf);
}