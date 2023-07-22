// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

/* #region SweetAlert components */

const Toast = Swal.mixin({
  toast: true,
  position: "top-right",
  showConfirmButton: false,
  timer: 1500,
  timerProgressBar: true,
  iconColor: "white",
  customClass: {
    popup: "colored-toast",
  },
  showClass: {
    popup: "animate__animated animate__fadeInDown",
  },
  hideClass: {
    popup: "animate__animated animate__fadeOutUp",
  },
  didOpen: (toast) => {
    toast.addEventListener("mouseenter", Swal.stopTimer);
    toast.addEventListener("mouseleave", Swal.resumeTimer);
  },
});

const DeleteToast = Toast.mixin({
  icon: 'error',
  iconHtml: '<i class="bi bi-trash"></i>',
  timer: 3000,
  customClass: {
    popup: "colored-toast",
    icon: "no-border",
  },
});

const DeleteAlert = Swal.mixin({
  icon: "warning",
  showCancelButton: true,
  reverseButtons: true,
  buttonsStyling: false,
  confirmButtonText: '<i class="bi bi-trash"></i> Delete',
  customClass: {
    cancelButton: "btn btn-outline-secondary mx-2",
    confirmButton: "btn btn-danger mx-2",
  },
});

/* #endregion SweetAlert components */

/* #region SweetAlert functions */

async function testSwalToasts() {
  await Toast.fire({
    icon: "success",
    title: "Success",
  });
  await Toast.fire({
    icon: "error",
    title: "Error",
  });
  await Toast.fire({
    icon: "warning",
    title: "Warning",
  });
  await Toast.fire({
    icon: "info",
    title: "Info",
  });
  await Toast.fire({
    icon: "question",
    title: "Question",
  });
  await Toast.fire({
    title: "Without icon / background-color",
    text: "This has subtext",
    timer: 3000
  });
  await Toast.fire({
    title: "Custom background",
    background: "coral",
  });
  await DeleteToast.fire({
    title: "DeleteToast",
    text: "Custom icon with no border",
  });
}

async function testSwalDelete() {
  await DeleteAlert.fire({
    title: 'Are you sure?',
    html: 'Object is going to be deleted!<br/>Do not use backdrop.',
    hideClass: {
        backdrop: 'swal2-backdrop-hide',
    }
  }).then((result) => {
    if (result.isConfirmed) {
      DeleteToast.fire({
        title: 'Object is deleted.',
        text: 'Object properties',
      });
    }
  });
}

async function fireToast(iconParam, toastTitleParam, toastTextParam) {
    return Toast.fire({
        icon: iconParam,
        title: toastTitleParam,
        text: toastTextParam,
    });
}

async function fireToastDelete(toastTitleParam, toastTextParam) {
    return DeleteToast.fire({
        icon: 'error',
        title: toastTitleParam,
        text: toastTextParam,
    });
}

async function fireSwalDelete(alertTextParam, toastTitleParam, toastTextParam) {
  await DeleteAlert.fire({
    title: "Are you sure?",
    text: alertTextParam,
  }).then((result) => {
    if (result.isConfirmed) {
        DeleteToast.fire({
        timerProgressBar: false,
        title: toastTitleParam,
        text: toastTextParam
      });
    }
  });
}
/* #endregion SweetAlert functions */
