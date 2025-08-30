import api from "./api";

export const getMidias = () => api.get("/midias");
export const createMidia = (data: FormData) =>
  api.post("/midias", data, {
    headers: { "Content-Type": "multipart/form-data" },
  });
export const updateMidia = (id: number, payload: FormData) =>
  api.put(`/midias/${id}`, payload, {
    headers: { "Content-Type": "multipart/form-data" },
  });
export const deleteMidia = (id: number) => api.delete(`/midias/${id}`);
