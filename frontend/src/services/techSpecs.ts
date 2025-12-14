import { apiClient } from "@utils/api";

export interface TechnicalSpecsEntity {
  id: string;
  key: string;
  value: string;
}

export async function getAllTechSpecs(): Promise<TechnicalSpecsEntity[]> {
  const data = await apiClient.get<TechnicalSpecsEntity[]>("/technicalspecs");
  return data || [];
}

export async function getTechSpec(
  id: string
): Promise<TechnicalSpecsEntity | undefined> {
  return await apiClient.get<TechnicalSpecsEntity>(`/technicalspecs/${id}`);
}

export async function createTechSpec(
  spec: TechnicalSpecsEntity
): Promise<TechnicalSpecsEntity | undefined> {
  return await apiClient.post<TechnicalSpecsEntity>("/technicalspecs", spec);
}

export async function updateTechSpec(
  id: string,
  spec: Partial<TechnicalSpecsEntity>
): Promise<TechnicalSpecsEntity | undefined> {
  return await apiClient.put<TechnicalSpecsEntity>(
    `/technicalspecs/${id}`,
    spec
  );
}

export async function deleteTechSpec(id: string): Promise<void> {
  await apiClient.delete(`/technicalspecs/${id}`);
}
