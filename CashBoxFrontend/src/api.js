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

export async function register(data) {
  const mapped = mapUserPayload(data);
  console.log('Register service payload:', mapped);
  return api.post('/api/auth/register', mapped);
}

export async function login(data) {
  return api.post('/api/auth/login', mapAuthPayload(data));
}

export async function getUsers() {
  return api.get('/api/user/getlist');
}

export async function getMe() {
  return api.get('/api/auth/getme');
}

export async function getRoles() {
  return api.get('/api/role/getlist');
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
  return api.post('/api/userrole/create', mapped);
}

export async function assignUserRoles(userId, roleIds) {
  return api.post('/api/userrole/AssignRole', roleIds, {
    params: { userId }
  });
}

export async function removeUserRole(userId, roleId) {
  return api.delete('/api/userrole/Remove', {
    params: { userId, roleId }
  });
}

export async function getUserRoles(userId) {
  return api.get('/api/userrole/GetList', {
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
  // Backenddan kelishi mumkin bo'lgan turli formatlarni unwrap qilish
  return body.data ?? body.items ?? body.value ?? body;
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
  return api.post('/api/user/create', mapped);
}

export async function updateUser(id, data) {
  return api.put(`/api/user/update/${id}`, mapUserPayload(data));
}

export async function deleteUser(id) {
  return api.delete(`/api/user/delete/${id}`);
}

export async function getOrganizations(params = {}) { // Umumiy getOrganizations funksiyasi
  return api.get('/api/organization/getlist', { params });
}

export async function getOrganizationsForRegister(params = {}) { // Faqat register uchun
  return api.get('/api/organization/GetListForRegister', { params });
}

export async function getOrganizationById(id) {
  return api.get(`/api/organization/get/${id}`);
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
  return api.post('/api/organization/create', mapped);
}

export async function updateOrganization(id, data) {
  return api.put(`/api/organization/update/${id}`, mapOrganizationPayload(data));
}

export async function deleteOrganization(id) {
  return api.delete(`/api/organization/delete/${id}`);
}

export async function getCurrencies() {
  return api.get('/api/currency/getlist');
}

export async function getCurrencyById(id) {
  return api.get(`/api/currency/get/${id}`);
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
  return api.post('/api/currency/create', mapped);
}

export async function updateCurrency(id, data) {
  return api.put(`/api/currency/update/${id}`, mapCurrencyPayload(data));
}

export async function deleteCurrency(id) {
  return api.delete(`/api/currency/delete/${id}`);
}

export async function getRegions() {
  return api.get('/api/region/getlist');
}

export async function getRegionById(id) {
  return api.get(`/api/region/get/${id}`);
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
  return api.post('/api/region/create', mapRegionPayload(data));
}

export async function updateRegion(id, data) {
  return api.put(`/api/region/update/${id}`, mapRegionPayload(data));
}

export async function deleteRegion(id) {
  return api.delete(`/api/region/delete/${id}`);
}

export async function getDistricts(regionId = null) {
  return api.get('/api/district/getlist', {
    params: regionId ? { regionId } : {}
  });
}

export async function getDistrictById(id) {
  return api.get(`/api/district/get/${id}`);
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
  return api.post('/api/district/create', mapDistrictPayload(data));
}

export async function updateDistrict(id, data) {
  return api.put(`/api/district/update/${id}`, mapDistrictPayload(data));
}

export async function deleteDistrict(id) {
  return api.delete(`/api/district/delete/${id}`);
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
export async function getSuppliers(params = {}) { return api.get('/api/supplier/getlist', { params }); }
export async function getSupplierById(id) { return api.get(`/api/supplier/get/${id}`); }
export async function createSupplier(data) { return api.post('/api/supplier/create', mapSupplierPayload(data)); }
export async function updateSupplier(id, data) { return api.put(`/api/supplier/update/${id}`, mapSupplierPayload(data)); }
export async function deleteSupplier(id) { return api.delete(`/api/supplier/delete/${id}`); }

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
export async function getProducts(params = {}) { return api.get('/api/product/getlist', { params }); }
export async function getProductById(id) { return api.get(`/api/product/get/${id}`); }
export async function createProduct(data) { return api.post('/api/product/create', mapProductPayload(data)); }
export async function updateProduct(id, data) { return api.put(`/api/product/update/${id}`, mapProductPayload(data)); }
export async function deleteProduct(id) { return api.delete(`/api/product/delete/${id}`); }

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
export async function getIncomeDocuments(params = {}) { return api.get('/api/incomedocument/getlist', { params }); }
export async function getIncomeDocumentById(id) { return api.get(`/api/incomedocument/get/${id}`); }
export async function createIncomeDocument(data) { return api.post('/api/incomedocument/create', mapIncomeDocumentPayload(data)); }
export async function updateIncomeDocument(id, data) { return api.put(`/api/incomedocument/update/${id}`, mapIncomeDocumentPayload(data)); }
export async function deleteIncomeDocument(id) { return api.delete(`/api/incomedocument/delete/${id}`); }

// ========== OUTCOME DOCUMENTS ==========
export async function getOutcomeDocuments() { return api.get('/api/OutcomeDocument/GetList'); }
export async function getOutcomeDocumentById(id) { return api.get(`/api/OutcomeDocument/Get/${id}`); }
export async function createOutcomeDocument(data) { return api.post('/api/OutcomeDocument/Create', data); }
export async function updateOutcomeDocument(id, data) { return api.put(`/api/OutcomeDocument/Update?id=${id}`, data); }
export async function deleteOutcomeDocument(id) { return api.delete(`/api/OutcomeDocument/Delete?id=${id}`); }