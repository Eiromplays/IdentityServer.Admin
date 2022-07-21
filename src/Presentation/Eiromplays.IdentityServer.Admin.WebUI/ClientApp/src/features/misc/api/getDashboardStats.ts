import { useQuery } from '@tanstack/react-query';
import { axios, ExtractFnReturnType, QueryConfig } from 'eiromplays-ui';

import { DashboardStats } from '../types';

export const getDashboardStats = (): Promise<DashboardStats> => {
  return axios.get(`/dashboard/stats`);
};

export type QueryFnType = typeof getDashboardStats;

export type UseStatsOptions = {
  config?: QueryConfig<QueryFnType>;
};

export const useDashboardStats = ({ config }: UseStatsOptions = {}) => {
  return useQuery<ExtractFnReturnType<QueryFnType>>({
    ...config,
    queryKey: ['stats'],
    queryFn: getDashboardStats,
  });
};
