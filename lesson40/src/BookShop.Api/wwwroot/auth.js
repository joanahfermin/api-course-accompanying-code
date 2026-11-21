async function apiFetch(url, options) {
  options = options ?? {};
  options.headers = options.headers ?? {};

  const token = sessionStorage.getItem('token');
  if (token) {
    options.headers['Authorization'] = 'Bearer ' + token;
  }

  const response = await fetch(url, options);
  if (response.status === 401) {
    location.href = '/login.html';
  }
  return response;
}

function logOut() {
  sessionStorage.clear();
  location.href = '/login.html';
}
