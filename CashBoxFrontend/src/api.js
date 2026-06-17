import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5107',
  headers: {
    'Content-Type': 'application/json'
  },
  withCredentials: false
});

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers = config.headers || {};
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  response => response,
  error => {
    const isAuthPage = window.location.pathname.startsWith('/auth/');

    if (!error.response) {
      console.error('Tarmoq xatosi yoki serverga ulanib bo\'lmadi:', error.message);
      return Promise.reject(error);
    }
    
    if (error.response?.status === 401 && !isAuthPage) {
      localStorage.removeItem('token');
      window.location.href = '/auth/login';
    }
    return Promise.reject(error);
  }
);

export async function getWeather(name) {
  console.log('getWeather function in api.js called with name:', name);
  return api.get('/api/Weather/Get', { params: { name } });
}

export async function register(data) {
  const mapped = mapUserPayload(data);
  console.log('Register service payload:', mapped);
  return api.post('/api/Auth/Register', mapped);
}

export async function login(data) {
  return api.post('/api/Auth/Login', mapAuthPayload(data));
}

export async function getUsers() {
  return api.get('/api/User/GetList');
}

export async function getMe() {
  return api.get('/api/Auth/GetMe');
}

export async function getRoles() {
  return api.get('/api/Role/GetList');
}

export async function getStates() {
  return api.get('/api/state/getlist');
}

const mapUserRolePayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    userId: 'UserId',
    roleId: 'RoleId',
    stateId: 'StateId',
    createUserId: 'CreateUserId',
    createdAt: 'CreatedAt',
    modifiedUserId: 'ModifiedUserId',
    modifiedAt: 'ModifiedAt'
  };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    const mappedKey = mapping[key] || key;
    result[mappedKey] = value;
    return result;
  }, {});
};

export async function createUserRole(data) {
  const mapped = mapUserRolePayload(data);
  return api.post('/api/UserRole/Create', mapped);
}

export async function assignUserRoles(userId, roleIds) {
  return api.post('/api/UserRole/AssignRole', roleIds, {
    params: { userId }
  });
}

export async function removeUserRole(userId, roleId) {
  return api.delete('/api/UserRole/Remove', {
    params: { userId, roleId }
  });
}

export async function getUserRoles(userId) {
  return api.get('/api/UserRole/GetList', {
    params: { userId }
  });
}

export const getField = (obj, keys) => {
  if (!obj) return null;
  for (const key of keys) {
    if (obj[key] !== undefined && obj[key] !== null) {
      return obj[key];
    }
  }
  return null;
};

export const extractApiData = (response) => {
  if (!response || typeof response !== 'object') return response;
  const body = response.data !== undefined ? response.data : response;
  if (!body) return null;
  if (Array.isArray(body)) return body;
  // Backenddan kelishi mumkin bo'lgan turli formatlarni (data, items, value yoki list) unwrap qilish
  return body.data ?? body.items ?? body.value ?? body.list ?? body;
};

export const normalizeUser = (data) => {
  if (!data || typeof data !== 'object') return null;
  return {
    id: getField(data, ['id', 'Id', 'userId', 'userID']),
    userName: getField(data, ['userName', 'UserName', 'username', 'Username', 'userName']),
    email: getField(data, ['email', 'Email', 'email']),
    fullName: getField(data, ['fullName', 'FullName', 'fullName']),
    shortName: getField(data, ['shortName', 'ShortName']),
    pinfl: getField(data, ['pinfl', 'Pinfl']),
    phoneNumber: getField(data, ['phoneNumber', 'PhoneNumber']),
    address: getField(data, ['address', 'Address']),
    organizationId: getField(data, ['organizationId', 'OrganizationId']),
    dateOfBirth: getField(data, ['dateOfBirth', 'DateOfBirth']),
    passportSeries: getField(data, ['passportSeries', 'PassportSeries']),
    role: getField(data, ['role', 'Role'])
  };
};

const mapAuthPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    userName: 'UserName',
    password: 'Password',
    email: 'Email'
  };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    const mappedKey = mapping[key] || key;
    result[mappedKey] = value;
    return result;
  }, {});
};

const mapUserPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    userName: 'UserName',
    password: 'Password',
    fullName: 'FullName',
    shortName: 'ShortName',
    pinfl: 'Pinfl',
    phoneNumber: 'PhoneNumber',
    address: 'Address',
    organizationId: 'OrganizationId',
    dateOfBirth: 'DateOfBirth',
    passportSeries: 'PassportSeries',
    email: 'Email',
    id: 'Id',
    role: 'Role'
  };
  const result = {};
  Object.entries(data).forEach(([key, value]) => {
    if (value === undefined) return;
    const mappedKey = mapping[key] || key;
    if (key === 'organizationId') {
      result[mappedKey] = parseInt(value) || 0;
    } else {
      result[mappedKey] = value;
    }
  });
  return result;
};

export async function createUser(data) {
  const mapped = mapUserPayload(data);
  console.log('User create payload:', mapped);
  return api.post('/api/User/Create', mapped);
}

export async function updateUser(id, data) {
  return api.put(`/api/User/Update/${id}`, mapUserPayload(data));
}

export async function deleteUser(id) {
  return api.delete(`/api/User/Delete/${id}`);
}

export async function getOrganizations(params = {}) { // Umumiy getOrganizations funksiyasi
  return api.get('/api/Organization/GetList', { params });
}

export async function getOrganizationsForRegister(params = {}) { // Faqat register uchun
  return api.get('/api/Organization/GetListForRegister', { params });
}

export async function getOrganizationById(id) {
  return api.get(`/api/Organization/Get/${id}`);
}

const mapOrganizationPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    inn: 'Inn',
    fullName: 'FullName',
    shortName: 'ShortName',
    regionId: 'RegionId',
    district: 'District',
    id: 'Id'
  };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    const mappedKey = mapping[key] || key;
    result[mappedKey] = value;
    return result;
  }, {});
};

export async function createOrganization(data) {
  const mapped = mapOrganizationPayload(data);
  console.log('Organization create payload:', mapped);
  return api.post('/api/Organization/Create', mapped);
}

export async function updateOrganization(id, data) {
  return api.put(`/api/Organization/Update/${id}`, mapOrganizationPayload(data));
}

export async function deleteOrganization(id) {
  return api.delete(`/api/Organization/Delete/${id}`);
}

export async function getCurrencies() {
  return api.get('/api/Currency/GetList');
}

export async function getCurrencyById(id) {
  return api.get(`/api/Currency/Get/${id}`);
}

const mapCurrencyPayload = (data) => {
  return {
    FullName: data.name || data.fullName || data.FullName,
    ShortName: data.symbol || data.shortName || data.ShortName,
    Code: data.code || data.Code,
  };
};

export async function createCurrency(data) {
  const { id, Id, ...rest } = data;
  const mapped = mapCurrencyPayload(rest);
  console.log('Currency create payload:', mapped);
  return api.post('/api/Currency/Create', mapped);
}

export async function updateCurrency(id, data) {
  return api.put(`/api/Currency/Update/${id}`, mapCurrencyPayload(data));
}

export async function deleteCurrency(id) {
  return api.delete(`/api/Currency/Delete/${id}`);
}

export async function getRegions() {
  return api.get('/api/Region/GetList');
}

export async function getRegionById(id) {
  return api.get(`/api/Region/Get/${id}`);
}

const mapRegionPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    fullName: 'FullName',
    shortName: 'ShortName',
    code: 'Code',
    id: 'Id'
  };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    const mappedKey = mapping[key] || key;
    result[mappedKey] = value;
    return result;
  }, {});
};

export async function createRegion(data) {
  return api.post('/api/Region/Create', mapRegionPayload(data));
}

export async function updateRegion(id, data) {
  return api.put(`/api/Region/Update/${id}`, mapRegionPayload(data));
}

export async function deleteRegion(id) {
  return api.delete(`/api/Region/Delete/${id}`);
}

export async function getDistricts(regionId = null) {
  return api.get('/api/District/GetList', {
    params: regionId ? { regionId } : {}
  });
}

export async function getDistrictById(id) {
  return api.get(`/api/District/Get/${id}`);
}

const mapDistrictPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = {
    fullName: 'FullName',
    code: 'Code',
    regionId: 'RegionId',
    id: 'Id'
  };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    const mappedKey = mapping[key] || key;
    result[mappedKey] = value;
    return result;
  }, {});
};

export async function createDistrict(data) {
  return api.post('/api/District/Create', mapDistrictPayload(data));
}

export async function updateDistrict(id, data) {
  return api.put(`/api/District/Update/${id}`, mapDistrictPayload(data));
}

export async function deleteDistrict(id) {
  return api.delete(`/api/District/Delete/${id}`);
}
// ========== SUPPLIERS ==========
const mapSupplierPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = { code: 'Code', inn: 'Inn', id: 'Id' };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    result[mapping[key] || key] = value;
    return result;
  }, {});
};
export async function getSuppliers(params = {}) { return api.get('/api/Supplier/GetList', { params }); }
export async function getSupplierById(id) { return api.get(`/api/Supplier/Get/${id}`); }
export async function createSupplier(data) { return api.post('/api/Supplier/Create', mapSupplierPayload(data)); }
export async function updateSupplier(id, data) { return api.put(`/api/Supplier/Update/${id}`, mapSupplierPayload(data)); }
export async function deleteSupplier(id) { return api.delete(`/api/Supplier/Delete/${id}`); }

// ========== PRODUCTS ==========
const mapProductPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = { name: 'Name', code: 'Code', organizationId: 'OrganizationId', deliveredAt: 'DeliveredAt', id: 'Id' };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    result[mapping[key] || key] = value;
    return result;
  }, {});
};
export async function getProducts(params = {}) { return api.get('/api/Product/GetList', { params }); }
export async function getProductById(id) { return api.get(`/api/Product/Get/${id}`); }
export async function createProduct(data) { return api.post('/api/Product/Create', mapProductPayload(data)); }
export async function updateProduct(id, data) { return api.put(`/api/Product/Update/${id}`, mapProductPayload(data)); }
export async function deleteProduct(id) { return api.delete(`/api/Product/Delete/${id}`); }

// ========== INCOME DOCUMENTS ==========
const mapIncomeDocumentPayload = (data) => {
  if (!data || typeof data !== 'object') return data;
  const mapping = { date: 'Date', supplierId: 'SupplierId', productId: 'ProductId', organizationId: 'OrganizationId', price: 'Price', quantity: 'Quantity', totalSum: 'TotalSum', id: 'Id' };
  return Object.entries(data).reduce((result, [key, value]) => {
    if (value === undefined) return result;
    result[mapping[key] || key] = value;
    return result;
  }, {});
};
export async function getIncomeDocuments(params = {}) { return api.get('/api/IncomeDocument/GetList', { params }); }
export async function getIncomeDocumentById(id) { return api.get(`/api/IncomeDocument/Get/${id}`); }
export async function createIncomeDocument(data) { return api.post('/api/IncomeDocument/Create', mapIncomeDocumentPayload(data)); }
export async function updateIncomeDocument(id, data) { return api.put(`/api/IncomeDocument/Update/${id}`, mapIncomeDocumentPayload(data)); }
export async function deleteIncomeDocument(id) { return api.delete(`/api/IncomeDocument/Delete/${id}`); }

// ========== OUTCOME DOCUMENTS ==========
export async function getOutcomeDocuments() { return api.get('/api/OutcomeDocument/GetList'); }
export async function getOutcomeDocumentById(id) { return api.get(`/api/OutcomeDocument/Get/${id}`); }
export async function createOutcomeDocument(data) { return api.post('/api/OutcomeDocument/Create', data); }
export async function updateOutcomeDocument(id, data) { return api.put(`/api/OutcomeDocument/Update?id=${id}`, data); }
export async function deleteOutcomeDocument(id) { return api.delete(`/api/OutcomeDocument/Delete?id=${id}`); }

export { api };