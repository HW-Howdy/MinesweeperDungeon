using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MineMapViewer : MonoBehaviour
{
	[Header("UI References")]
	[SerializeField]
	private ScrollRect _scrollRect;
	[SerializeField]
	private RectTransform _content;
	[SerializeField]
	private GameObject _cellPrefab;
	[SerializeField]
	private TMP_InputField _inputFieldCellSize;

	[Header("Grid Settings")]
	[SerializeField]
	private int cellSize = 100; // 각 칸 크기 (px)

	private int visibleRows;
	private int visibleCols;

	private List<GameObject>	cellObjs = new List<GameObject>();
	private List<CellView>		cellViews = new List<CellView>();
	private List<RectTransform>	cellRects = new List<RectTransform>();

	public void Start()
	{
		Resize(cellSize);
		_scrollRect.onValueChanged.AddListener((_) => UpdateCells());
		CellView.StaticActionAfterClick += UpdateCellView;
		GameManager.Instance.ActionOpenCellAfter += UpdateCellView;
		MineMapModel.Instance.ActionResizeMapAfter += Resize;
		MineMapModel.Instance.ActionReloadMap += UpdateCellView;
		return ;
	}

	public void OnDestroy()
	{
		CellView.StaticActionAfterClick -= UpdateCellView;
		return ;
	}

	public void ButtonResize()
	{
		if (!int.TryParse(_inputFieldCellSize.text, out int targetSize))
			targetSize = 50;
		Resize(targetSize);
		return ;
	}

	private void Resize()
	{
		Resize(cellSize);
		return ;
	}

	private void Resize(int targetSize)
	{
		cellSize = Math.Clamp(targetSize, 40, 200);
		_inputFieldCellSize.text = cellSize.ToString();
		// Content 크기를 전체 데이터 크기에 맞춤
		_content.sizeDelta = new Vector2(MineMapModel.Instance.Cols * cellSize, MineMapModel.Instance.Rows * cellSize);
		_content.localPosition = Vector2.zero;

		// Viewport에 보이는 셀 개수 계산 (+1 여유)
		RectTransform viewport = _scrollRect.viewport;
		visibleCols = Mathf.CeilToInt(viewport.rect.width / cellSize) + 1;
		visibleRows = Mathf.CeilToInt(viewport.rect.height / cellSize) + 1;

		// 풀링할 셀 생성
		int poolSize = visibleCols * visibleRows;
		int i = cellObjs.Count;
		while (i < poolSize)
		{
			GameObject cell = Instantiate(_cellPrefab, _content);

			cellObjs.Add(cell);
			cellViews.Add(cell.GetComponent<CellView>());
			cellRects.Add(cell.GetComponent<RectTransform>());
			i++;
		}
		while (--i > poolSize)
		{
			Destroy(cellObjs[i]);
			cellObjs.RemoveAt(i);
			cellViews.RemoveAt(i);
			cellRects.RemoveAt(i);
		}
		while (i >= 0)
		{
			cellRects[i--].sizeDelta = Vector2.one * cellSize;
		}
		UpdateCells();
		return ;
	}

	private void UpdateCells()
	{
		// 스크롤된 위치 계산
		float scrollY = _content.anchoredPosition.y;
		float scrollX = -_content.anchoredPosition.x;

		int firstRow = Mathf.FloorToInt(scrollY / cellSize);
		int firstCol = Mathf.FloorToInt(scrollX / cellSize);

		for (int i = 0; i < cellObjs.Count; i++)
		{
			int x = i / visibleCols + firstRow;
			int y = i % visibleCols + firstCol;

			if (x < 0 || x >= MineMapModel.Instance.Rows || y < 0 || y >= MineMapModel.Instance.Cols)
			{
				cellObjs[i].SetActive(false);
				continue ;
			}
			cellObjs[i].SetActive(true);
			cellRects[i].anchoredPosition = new Vector2(y * cellSize + cellSize / 2, - x * cellSize - cellSize / 2);
			cellViews[i].SetLocate(y, x);
			cellViews[i].SetAct(MineMapModel.Instance.CanOpen);
		}
		return ;
	}

	public void UpdateCellView()
	{
		float scrollY = _content.anchoredPosition.y;
		float scrollX = -_content.anchoredPosition.x;

		int firstRow = Mathf.FloorToInt(scrollY / cellSize);
		int firstCol = Mathf.FloorToInt(scrollX / cellSize);

		for (int i = 0; i < cellObjs.Count; i++)
		{
			int x = i / visibleCols + firstRow;
			int y = i % visibleCols + firstCol;
			if (x < 0 || x >= MineMapModel.Instance.Rows || y < 0 || y >= MineMapModel.Instance.Cols)
			{
				continue ;
			}
			cellViews[i].ReloadState(y, x);
			cellViews[i].GetComponent<CellView>().SetAct(MineMapModel.Instance.CanOpen);
		}
		return ;
	}
}
