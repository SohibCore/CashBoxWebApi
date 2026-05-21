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
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  response => response,
  error => {
    const isAuthPage = window.location.pathname.startsWith('/auth/');
    
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

export const extractApiData = (response) => {
  if (!response || typeof response !== 'object') return response;
  return response.data?.data ?? response.data ?? response;
};

export const normalizeUser = (data) => {
  if (!data || typeof data !== 'object') return null;
  const getField = (obj, keys) => {
    for (const key of keys) {
      if (obj[key] !== undefined && obj[key] !== null) {
        return obj[key];
      }
    }
    return null;
  };
  return {
    id: getField(data, ['id', 'Id', 'userId', 'userID']),
    userName: getField(data, ['userName', 'UserName', 'username', 'Username']),
    email: getField(data, ['email', 'Email']),
    fullName: getField(data, ['fullName', 'FullName']),
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
    // Ensure organizationId is a number
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

export async function getOrganizations(params = {}) {
  return api.get('/api/organization/getlist', { params });
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

const mapCurrencyPayload = (data) => { // O'zgartirilgan
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
