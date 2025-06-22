import { Button, TextField, Grid, Divider, Dialog, DialogTitle, DialogContent, TableContainer, Table, TableHead, TableRow, TableCell, TableBody, Typography, Box, Chip, IconButton, DialogActions } from '@mui/material';
import './appStyle.css';
import { useCallback, useEffect, useState } from 'react';
import { Toast, useToast } from './components';

function App() {
  const [rows, setRows] = useState(undefined);
  const [columns, setColumns] = useState(undefined);
  const [treasure, setTreasure] = useState(undefined);
  const [treasuresMap, setTreasuresMap] = useState([]);
  const [result, setResult] = useState(undefined);
  const [isLoading, setIsLoading] = useState(false);
  const [historyDialogOpen, setHistoryDialogOpen] = useState(false);
  const { toast, showError, hideToast } = useToast();
  //a

  const handleSubmit = (e) => {
    e.preventDefault();
    if(!rows || !columns || !treasure) {
      return showError("Vui lòng nhập đầy đủ thông tin");
    }
    // check validate treasures map
    const error = treasuresMap.flatMap((row)=>row.filter((cell)=>!validateTreasuresMap(cell)));
    if(error.length > 0) {
      return;
    }
    
    // call api find treasure
    setIsLoading(true);
    fetch(`${process.env.REACT_APP_API_URL}/api/TreasuareHunt`, {
      method: "POST",
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        row: rows,
        col: columns,
        treasure: treasure,
        treasure_Map: treasuresMap
      })
    }).then((res) => res.json()).then((data) => {
      setResult(data);
    }).catch((error) => {
      console.error('Error:', error);
      showError("Có lỗi xảy ra khi gọi API");
    }).finally(() => {
      setIsLoading(false);
    });
  }

  const validateTreasuresMap = useCallback((cell)=>{
    if(cell === undefined) {
      showError("Thiếu dữ liệu");
      return false;
    }
    if(cell < 1 ) {
      showError("Rương phải lớn hơn 1");
      return false;
    }
    if(cell > Number(treasure)) {
      showError("Rương phải nhỏ hơn Rương kho báu");
      return false;
    }

    if(cell === Number(treasure) && treasuresMap.filter((row)=>row.filter((cell)=>cell === treasure).length > 1).length > 0) {
      showError("Trong map chỉ tồn tại duy nhất 1 dương kho báu")
      return false;
    }
    return true;
  },[treasure])

  const handleViewHistory = (historyItem) => {
    console.log("historyItem",historyItem);
    setRows(historyItem.row);
    setColumns(historyItem.col);
    setTreasure(historyItem.treasure);
    setTreasuresMap(historyItem.treasure_Map);
    setResult({
      solution: {
        totalEnergyConsumed: historyItem.minimum_Path,
        nodes: historyItem.track_Path.map((node)=>({
          row: node.row,
          col: node.col
        }))
      }
    });
    setHistoryDialogOpen(false);
  }

  return (
    <div className="container">
      <div className="game-container">
        <div className="game-title">
          <h1>Tìm kho báu</h1>
        </div>
        <div className="game-content">
          <form onSubmit={handleSubmit}>
            <Grid container spacing={2}>
              <Grid item xs={12} md={4}>
                <TextField
                  label="Số Hàng"
                  type="number"
                  placeholder="Nhập số hàng"
                  value={rows}
                  onChange={(e) => setRows(e.target.value)}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <TextField
                  label="Số Cột"
                  placeholder="Nhập số cột"
                  value={columns}
                  onChange={(e) => setColumns(e.target.value)}
                />
              </Grid>
              <Grid item xs={12} md={4}>
                <TextField
                  label="Dương Kho Báu"
                  type="number"
                  placeholder="Nhập số của dương kho báu"
                  value={treasure}
                  onChange={(e) => setTreasure(e.target.value)}
                />
              </Grid>
              <Grid item xs={12} md={12}>
                <Button type='submit' variant='contained' color='primary' sx={{ height: '100%',paddingX: '20px' }} 
                loading={isLoading}
                >Tìm kho báu</Button>
              </Grid>
              <Grid item xs={12} md={12}>
                <Button variant='contained' color='primary' sx={{ height: '100%',paddingX: '20px' }} 
                onClick={()=>setHistoryDialogOpen(true)}
                >Xem lịch sử</Button>
              </Grid>
            </Grid>
            <Divider sx={{ margin: '15px 0' }} />
            <div className='treasures-map'>
              {
                Array.from({ length: rows }, (_, indexRow) => (
                  <div className='treasures-map-row' key={indexRow}>
                    {Array.from({ length: columns }, (_, indexColumn) => (
                      <TextField 
                        key={indexColumn}
                        className='treasures-map-cell'
                        inputProps={{
                          className: 'no-spinner',
                        }}
                        type="number"
                        value={treasuresMap?.[indexRow]?.[indexColumn]}
                        onChange={(e) => setTreasuresMap((prev) => {
                          let newMap = [...prev];

                          if(!prev || prev?.length != rows || prev?.[0]?.length != columns) {
                            newMap = Array.from({ length: rows }, () => Array.from({ length: columns }, () => undefined));
                          }
                          newMap[indexRow][indexColumn] = e.target.value;
                          return newMap;
                        })}
                        onBlur={()=>{
                         validateTreasuresMap(treasuresMap?.[indexRow]?.[indexColumn])
                        }}
                      />
                    ))}
                  </div>
                ))
              }
            </div>
            {result?.solution && (
              <>
                <p>Tổng năng lượng cần thiết: {Number(result?.solution?.totalEnergyConsumed).toFixed(6)}</p>
                <p>Những vị trí cần đi qua: {result?.solution?.nodes?.map((node)=> `[${node.row},${node.col}]`).join(" -> ")}</p>
              </>
            )}
          </form>
        </div>
      </div>
      
      {/* Toast Component */}
      <Toast
        open={toast.open}
        message={toast.message}
        severity={toast.severity}
        onClose={hideToast}
      />
      <History open={historyDialogOpen} onClose={()=>setHistoryDialogOpen(false)} handleViewHistory={handleViewHistory} />
    </div>
  );
}


function History({open, onClose , handleViewHistory}) {
  const [history, setHistory] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(()=>{
    if (open) {
      setLoading(true);
      fetch(`${process.env.REACT_APP_API_URL}/api/TreasuareHunt/history`)
      .then((res) => res.json())
      .then((data) => {
        setHistory(data.data || []);
        setLoading(false);
      })
      .catch((error) => {
        console.error('Error fetching history:', error);
        setLoading(false);
      });
    }
  },[open]);

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleString('vi-VN');
  };

  return (
    <>
      <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>  
        <DialogTitle>
          <Box display="flex" justifyContent="space-between" alignItems="center">
            <Typography variant="h6">Lịch sử tìm kho báu</Typography>
          </Box>
        </DialogTitle>
        <DialogContent>
          {loading ? (
            <Box display="flex" justifyContent="center" p={3}>
              <Typography>Đang tải...</Typography>
            </Box>
          ) : history.length === 0 ? (
            <Box display="flex" justifyContent="center" p={3}>
              <Typography color="text.secondary">Chưa có lịch sử nào</Typography>
            </Box>
          ) : (
            <TableContainer>
              <Table sx={{ minWidth: 650 }} aria-label="history table">
                <TableHead>
                  <TableRow>
                    <TableCell>ID</TableCell>
                    <TableCell>Số hàng</TableCell>
                    <TableCell>Số cột</TableCell>
                    <TableCell>Kho báu</TableCell>
                    <TableCell>Tổng năng lượng</TableCell>
                    <TableCell>Ngày tạo</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {history.map((item) => (
                    <TableRow key={item.id} hover onClick={()=>handleViewHistory(item)}>
                      <TableCell>{item.id}</TableCell>
                      <TableCell>
                        {item.row}
                      </TableCell>
                      <TableCell>
                        {item.col}
                      </TableCell>
                      <TableCell>
                        {item.treasure}
                      </TableCell>
                      <TableCell>
                        {Number(item.minimum_Path.toFixed(6))}
                      </TableCell>
                      <TableCell>
                        <Typography variant="body2">
                          {formatDate(item.createdAt)}
                        </Typography>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}
        </DialogContent>
      </Dialog>
    </>
  );
}

export default App;
