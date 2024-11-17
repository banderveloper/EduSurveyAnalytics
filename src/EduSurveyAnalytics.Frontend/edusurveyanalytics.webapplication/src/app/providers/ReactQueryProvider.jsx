import {QueryClient, QueryClientProvider} from "react-query"

const queryClient = new QueryClient();

export const ReactQueryProvider = (x) => {
    return (
        <QueryClientProvider client={queryClient}>
            {x.children}
        </QueryClientProvider>
    )
}