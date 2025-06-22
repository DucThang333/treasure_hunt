import { Button, TextField, Grid, Divider, Alert } from '@mui/material';
import './appStyle.css';
import { useCallback, useMemo, useState } from 'react';
import { Toast, useToast } from './components';

function App() {
  const [rows, setRows] = useState(undefined);
  const [columns, setColumns] = useState(undefined);
  const [treasure, setTreasure] = useState(undefined);
  const [treasuresMap, setTreasuresMap] = useState([]);
  const [result, setResult] = useState(undefined);
  const { toast, showError, hideToast } = useToast();
  //a

  const handleSubmit = (e) => {
    e.preventDefault();
    if(!rows || !columns || !treasure) {
      return showError("Vui lòng nhập đầy đủ thông tin");
    }
    // check validate treasures map
    const error = treasuresMap.find((row, indexRow) => (
      row.find((cell, indexColumn) => {
        if(!validateTreasuresMap(cell, indexRow, indexColumn)) {
          return true;
        }
      })
    ));
    if(error) {
      return;
    }
    
    // call api find treasure
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
    if(cell > treasure) {
      showError("Rương kho báu phải nhỏ hơn Rương kho báu");
      return false;
    }

    if(cell === treasure && treasuresMap.filter((row)=>row.filter((cell)=>cell === treasure).length > 1).length > 0) {
      showError("Trong map chỉ tồn tại duy nhất 1 dương kho báu")
      return false;
    }
    return true;
  },[treasure])

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
                  type="number"
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
                <Button type='submit' variant='contained' color='primary' sx={{ height: '100%',paddingX: '20px' }}>Tìm kho báu</Button>
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
    </div>
  );
}

export default App;
